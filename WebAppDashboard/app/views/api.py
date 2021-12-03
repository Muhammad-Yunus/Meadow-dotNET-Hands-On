from . import app
from . import request
from . import jsonify

# load sensor factory object
from . import factory


# Create Exposed API to receive sensor data
@app.route('/api/v1/sensor/post', methods=['POST'])
def sensor_post():
    try : 
        json_data = request.json
        name = json_data['name']
        device_no = json_data['device_no']
        temperature = float(json_data['temperature'])
        humidity = float(json_data['humidity'])
        # insert data
        factory.insertData(name, device_no, temperature, humidity)

        message = {
            'status': 200,
            'message': 'Success'
        }
        resp = jsonify(message)
        return resp

    except Exception as e:
        message = {
            'status': 500,
            'message': "[ERROR] " + str(e)
        }
        resp = jsonify(message)
        return resp

# Create Exposed API to receive sensor data
@app.route('/api/v1/bmp180/post', methods=['POST'])
def bmp180_post():
    try : 
        json_data = request.json

        name = json_data['name']
        device_no = json_data['device_no']
        temperature = float(json_data['temperature'])
        pressure = float(json_data['pressure'])
        # insert data
        factory.insertData(name, device_no, temperature, pressure)
        print("\nReceive : %s, %s, %.2fC, %.2fpsi\n" % (name, device_no, temperature, pressure))

        message = {
            'status': 200,
            'message': 'Success'
        }
        resp = jsonify(message)
        return resp

    except Exception as e:
        message = {
            'status': 500,
            'message': "[ERROR] " + str(e)
        }
        resp = jsonify(message)
        return resp

# Create Exposed API to receive sensor data
@app.route('/api/v1/mpu6050/post', methods=['POST'])
def mpu6050_post():
    try : 
        json_data = request.json

        name = json_data['name']
        device_no = json_data['device_no']
        acceleration = {
            'X' : float(json_data['acceleration']['X']),
            'Y' : float(json_data['acceleration']['Y']),
            'Z' : float(json_data['acceleration']['Z'])
        }
        angular_velocity = {
            'X' : float(json_data['angular_velocity']['X']),
            'Y' : float(json_data['angular_velocity']['Y']),
            'Z' : float(json_data['angular_velocity']['Z'])
        }
        temperature = float(json_data['temperature'])
        
        print("\nReceive : ")
        print("Acceleration", acceleration)
        print("Angula Velocity", angular_velocity)
        print("Temperature", temperature)
        print("\n")

        message = {
            'status': 200,
            'message': 'Success'
        }
        resp = jsonify(message)
        return resp

    except Exception as e:
        message = {
            'status': 500,
            'message': "[ERROR] " + str(e)
        }
        resp = jsonify(message)
        return resp

@app.errorhandler(404)
def not_found(error=None):
    message = {
        'status': 404,
        'message': 'Not Found: ' + request.url,
    }
    resp = jsonify(message)
    resp.status_code = 404

    return resp