import sys
import serial

#Based on code by StackOverflow user Thomas 
#Thread at this link: 
#https://stackoverflow.com/questions/12090503/listing-available-com-ports-with-python
def serial_ports():
    if sys.platform.startswith('win'):
        ports = ['COM%s' % (i + 1) for i in range(256)]
    else:
        raise EnvironmentError('Unsupported platform')

    result = []
    for port in ports:
        try:
            s = serial.Serial(port)
            s.close()
            result.append(port)
        except (OSError, serial.SerialException):
            pass
    return result

def choose_port(serial_port_list):
    print("Available Ports:")
    print(", ".join(serial_port_list))
    while(True):
        choice = input("Choose a port: ")
        try:
            choice = "COM{}".format(int(choice))
        except ValueError:
            pass
        if choice.upper() in serial_port_list:
            print("")
            return choice

def get_port():
    COM_PORTS = serial_ports()
    if len(COM_PORTS) == 0:
        print("No Com Ports detected")
        exit(-1)
    elif len(COM_PORTS) == 1:
        port = (COM_PORTS[0])
        print("Testing on port: {}".format(port))
    elif len(COM_PORTS) > 1:
        port = choose_port(COM_PORTS)
    return port