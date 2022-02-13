let fs = require('fs');
let express = require('express');
let db = require("../db");
let router = express.Router();
let { v4: uuidv4 } = require('uuid');

function streamToString(stream) {
    const chunks = [];
    return new Promise((resolve, reject) => {
        stream.on('data', chunk => chunks.push(chunk));
        stream.on('error', reject);
        stream.on('end', () => resolve(Buffer.concat(chunks)));
    })
}

/* GET index page */
router.get('/', function (req, res, next) {
    res.render('index', {title: 'PhotoGallery', version: "0.1.0"});
});

/* POST new image */
router.post('/new', async function (req, res, next) {
    console.log('Request fields:', req.body);
    console.log('Request files:', Object.getOwnPropertyNames(req.files));

    if (!req.files['image']) {
        res.sendStatus(400);
        res.send('image required');
        return;
    }

    const extension = "." + req.files['image'].path.split('.')[1];
    const fileName = uuidv4() + extension;
    const imageDataString = await streamToString(req.files['image']);

    fs.writeFileSync('./public/images/' + fileName, imageDataString);

    try {
        db.connection.query(`INSERT INTO data (name, description, author, path) 
                             VALUES ('${req.body['name']}', 
                                    '${req.body['description']}',
                                    '${req.body['author']}',
                                    '${fileName}' );`, function (err, rows, fields) {
            if (err) throw err;

            console.log(rows);
        });
    } catch (err) {
        res.sendStatus(500);
        res.send(err.toString());
        return;
    }

    res.send(fileName);
});

/* GET all images */
router.get('/all', async function (req, res, next) {
    db.connection.query('SELECT * from data', function (err, rows, fields) {
        if (err) {
            res.sendStatus(500);
            res.send(err);
        } else {
            res.send(rows);
        }
    });
});

module.exports = router;
