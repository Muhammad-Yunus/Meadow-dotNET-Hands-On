# Utils
from . import np
from . import datetime
from . import db 

# Model
from . import Sensor

from . import socketio, emit

class SensorDataFactory :
  def __init__(self):
    self.data = {
      'temperature' : 0,
      'pressure'    : 0,
      'time'        : datetime.now()
    }

  def insertData(self, name, device_no, temperature, pressure):

    self.data =  {
      'temperature' : np.around(temperature, decimals=2),
      'pressure'    : np.around(pressure, decimals=2),
      'time'        : datetime.now()
    }
    # save to db for presistent
    Temperature = Sensor(name=name,
          device_no=device_no,
          device_type='Temperature',
          time=self.data['time'],
          value=self.data['temperature']
          )
    Pressure = Sensor(name=name,
          device_no=device_no,
          device_type='Pressure',
          time=self.data['time'],
          value=self.data['pressure']
          )
    db.session.add(Temperature)      
    db.session.add(Pressure)
    db.session.commit()

    # call emit function
    self.emit_sensor_data()
    self.emit_pressure()
    self.emit_temperature()

  def emit_sensor_data(self): 
    msg = {}
    msg['label'] = self.data['time'].timestamp()*1000
    msg['datasets'] = [{
      'label' : 'Temperature',
      'data'  : {
                'y' : self.data['temperature'],
                'x' : self.data['time'].timestamp()*1000
                },
      'borderColor'         : '#f56954',
      'backgroundColor'     : '#f56954',
    },
    {
      'label' : 'Pressure',
      'data'  : {
                'y' : self.data['pressure'],
                'x' : self.data['time'].timestamp()*1000
                },
      'borderColor'         : '#00a65a',
      'backgroundColor'     : '#00a65a',
    }]
    socketio.emit('SensorHistoryEvent', msg)

  def emit_pressure(self):
    msg = {}
    msg['datasets'] = [{
      'value' : self.data['pressure']
    }]
    socketio.emit('PressureEvent', msg)

  def emit_temperature(self):
    msg = {}
    msg['datasets'] = [{
      'value' : self.data['temperature']
    }]
    socketio.emit('TemperatureEvent', msg)    