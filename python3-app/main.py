#!/usr/bin/env python3

import logging
import os
import redis
import tornado.ioloop
import tornado.web

from tornado.options import parse_command_line

PORT = 8888
r = redis.StrictRedis(host=os.environ.get("REDIS_HOST", "localhost"), 
    port=int(os.environ.get("REDIS_PORT", "6379")), db=0)


class MainHandler(tornado.web.RequestHandler):
    def get(self):
        self.render('templates/index.html')


class HospitalHandler(tornado.web.RequestHandler):
    def get(self):
        items = []
        try:
            ID = r.get("hospital:autoID").decode()

            for i in range(int(ID)):
                result = r.hgetall("hospital:" + str(i))
                if result:
                    items.append(result)

        except redis.exceptions.ConnectionError:
            self.set_status(400)
            self.write("Redis connection refused")
        else:
            self.render('templates/hospital.html', items=items)

    def post(self):
        name = self.get_argument('name')
        address = self.get_argument('address')
        beds_number = self.get_argument('beds_number')
        phone = self.get_argument('phone')

        if not name or not address:
            self.set_status(400)
            self.write("Hospital name and address required")
            return

        logging.debug(name + ' ' + address + ' ' + beds_number + ' ' + phone)

        try:
            ID = r.get("hospital:autoID").decode()

            a  = r.hset("hospital:" + ID, "name", name)
            a += r.hset("hospital:" + ID, "address", address)
            a += r.hset("hospital:" + ID, "phone", phone)
            a += r.hset("hospital:" + ID, "beds_number", beds_number)

            r.incr("hospital:autoID")
        except redis.exceptions.ConnectionError:
            self.set_status(400)
            self.write("Redis connection refused")
        else:
            if (a != 4):
                self.set_status(500)
                self.write("Something went terribly wrong")
            else:
                self.write('OK: ID ' + ID + " for " + name)


class DoctorHandler(tornado.web.RequestHandler):
    def get(self):
        items = []
        try:
            ID = r.get("doctor:autoID").decode()

            for i in range(int(ID)):
                result = r.hgetall("doctor:" + str(i))
                if result:
                    items.append(result)

        except redis.exceptions.ConnectionError:
            self.set_status(400)
            self.write("Redis connection refused")
        else:
            self.render('templates/doctor.html', items=items)

    def post(self):
        surname = self.get_argument('surname')
        profession = self.get_argument('profession')
        hospital_ID = self.get_argument('hospital_ID')

        if not surname or not profession:
            self.set_status(400)
            self.write("Surname and profession required")
            return

        logging.debug(surname + ' ' + profession)

        try:
            ID = r.get("doctor:autoID").decode()

            if hospital_ID:
                # check that hospital exist
                hospital = r.hgetall("hospital:" + hospital_ID)

                if not hospital:
                    self.set_status(400)
                    self.write("No hospital with such ID")
                    return

            a  = r.hset("doctor:" + ID, "surname", surname)
            a += r.hset("doctor:" + ID, "profession", profession)
            a += r.hset("doctor:" + ID, "hospital_ID", hospital_ID)

            r.incr("doctor:autoID")
        except redis.exceptions.ConnectionError:
            self.set_status(400)
            self.write("Redis connection refused")
        else:
            if (a != 3):
                self.set_status(500)
                self.write("Something went terribly wrong")
            else:
                self.write('OK: ID ' + ID + " for " + surname)


class PatientHandler(tornado.web.RequestHandler):
    def get(self):
        items = []
        try:
            ID = r.get("patient:autoID").decode()

            for i in range(int(ID)):
                result = r.hgetall("patient:" + str(i))
                if result:
                    items.append(result)

        except redis.exceptions.ConnectionError:
            self.set_status(400)
            self.write("Redis connection refused")
        else:
            self.render('templates/patient.html', items=items)

    def post(self):
        surname = self.get_argument('surname')
        born_date = self.get_argument('born_date')
        sex = self.get_argument('sex')
        mpn = self.get_argument('mpn')

        if not surname or not born_date or not sex or not mpn:
            self.set_status(400)
            self.write("All fields required")
            return

        # wow what a check
        if sex not in ['M', 'F']:
            self.set_status(400)
            self.write("Sex must be 'M' or 'F'")
            return

        logging.debug(surname + ' ' + born_date + ' ' + sex + ' ' + mpn)

        try:
            ID = r.get("patient:autoID").decode()

            a  = r.hset("patient:" + ID, "surname", surname)
            a += r.hset("patient:" + ID, "born_date", born_date)
            a += r.hset("patient:" + ID, "sex", sex)
            a += r.hset("patient:" + ID, "mpn", mpn)

            r.incr("patient:autoID")
        except redis.exceptions.ConnectionError:
            self.set_status(400)
            self.write("Redis connection refused")
        else:
            if (a != 4):
                self.set_status(500)
                self.write("Something went terribly wrong")
            else:
                self.write('OK: ID ' + ID + " for " + surname)


class DiagnosisHandler(tornado.web.RequestHandler):
    def get(self):
        items = []
        try:
            ID = r.get("diagnosis:autoID").decode()

            for i in range(int(ID)):
                result = r.hgetall("diagnosis:" + str(i))
                if result:
                    items.append(result)
            
        except redis.exceptions.ConnectionError:
            self.set_status(400)
            self.write("Redis connection refused")
        else:
            self.render('templates/diagnosis.html', items=items)

    def post(self):
        patient_ID = self.get_argument('patient_ID')
        diagnosis_type = self.get_argument('type')
        information = self.get_argument('information')

        if not patient_ID or not diagnosis_type:
            self.set_status(400)
            self.write("Patiend ID and diagnosis type required")
            return

        logging.debug(patient_ID + ' ' + diagnosis_type + ' ' + information)

        try:
            ID = r.get("diagnosis:autoID").decode()

            patient = r.hgetall("patient:" + patient_ID)

            if not patient:
                self.set_status(400)
                self.write("No patient with such ID")
                return

            a  = r.hset("diagnosis:" + ID, "patient_ID", patient_ID)
            a += r.hset("diagnosis:" + ID, "type", diagnosis_type)
            a += r.hset("diagnosis:" + ID, "information", information)

            r.incr("diagnosis:autoID")
        except redis.exceptions.ConnectionError:
            self.set_status(400)
            self.write("Redis connection refused")
        else:
            if (a != 3):
                self.set_status(500)
                self.write("Something went terribly wrong")
            else:
                self.write('OK: ID ' + ID + " for patient " + patient[b'surname'].decode())


class DoctorPatientHandler(tornado.web.RequestHandler):
    def get(self):
        items = {}
        try:
            ID = r.get("doctor:autoID").decode()

            for i in range(int(ID)):
                result = r.smembers("doctor-patient:" + str(i))
                if result:
                    items[i] = result

        except redis.exceptions.ConnectionError:
            self.set_status(400)
            self.write("Redis connection refused")
        else:
            self.render('templates/doctor-patient.html', items=items)

    def post(self):
        doctor_ID = self.get_argument('doctor_ID')
        patient_ID = self.get_argument('patient_ID')
        
        if not doctor_ID or not patient_ID:
            self.set_status(400)
            self.write("ID required")
            return

        logging.debug(doctor_ID + ' ' + patient_ID)

        try:
            patient = r.hgetall("patient:" + patient_ID)
            doctor = r.hgetall("doctor:" + doctor_ID)

            if not patient or not doctor:
                self.set_status(400)
                self.write("No such ID for doctor or patient")
                return

            r.sadd("doctor-patient:" + doctor_ID, patient_ID)

        except redis.exceptions.ConnectionError:
            self.set_status(400)
            self.write("Redis connection refused")
        else:
            self.write("OK: doctor ID: " + doctor_ID + ", patient ID: " + patient_ID)


def init_db():
    db_initiated = r.get("db_initiated")
    if not db_initiated:
        r.set("hospital:autoID", 1)
        r.set("doctor:autoID", 1)
        r.set("patient:autoID", 1)
        r.set("diagnosis:autoID", 1)
        r.set("db_initiated", 1)


def make_app():
    return tornado.web.Application([
        (r"/", MainHandler),
        (r'/static/(.*)', tornado.web.StaticFileHandler, {'path': 'static/'}),
        (r"/hospital", HospitalHandler),
        (r"/doctor", DoctorHandler),
        (r"/patient", PatientHandler),
        (r"/diagnosis", DiagnosisHandler),
        (r"/doctor-patient", DoctorPatientHandler)
    ], autoreload=True, debug=True, compiled_template_cache=False, serve_traceback=True)


if __name__ == "__main__":
    init_db()
    app = make_app()
    app.listen(PORT)
    tornado.options.parse_command_line()
    logging.info("Listening on " + str(PORT))
    tornado.ioloop.IOLoop.current().start()