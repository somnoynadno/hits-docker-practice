const mysql = require('mysql');
const connection = mysql.createConnection({
    host: 'localhost',
    user: 'mysql',
    password: 'mysql',
    database: 'db'
});

module.exports = { connection };
