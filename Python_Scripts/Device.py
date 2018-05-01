from MongoInterface import *
import random
import json

drop_code = "DROP"
garble_code = "GARBLE"
normal_code = "NORMAL"

class ValueRange:
	MIN = 0
	MAX = 0
	RANGE = 0
	def __init__(self, minimum, maximum):
		if maximum < minimum:
			raise ValueError("Maximum ({}) must be greater than minimum ({})".format(maximum, minimum))
		self.MIN = minimum
		self.MAX = maximum
		self.RANGE = self.MAX - self.MIN

	def percentile(self, val):
		if val < self.MIN:
			return 0.0
		elif val > self.MAX:
			return 1.0
		else:
			return ((val - self.MIN) / float(self.RANGE))

	def getPercentile(self, percentile):
		return ((self.RANGE * percentile) + self.MIN)

heartHardRange = ValueRange(0, 200)
heartSoftRange = ValueRange(30, 140)
heartSwingRange = ValueRange(0.02, 0.1)

bloodHardRange = ValueRange(0, 100)
bloodSoftRange = ValueRange(75.0, 89.0)
bloodSwingRange = ValueRange(0.01, 0.02)

respHardRange = ValueRange(0, 240)
respSoftRange = ValueRange(5, 80)
respSwingRange = ValueRange(0.01, 0.06)


def randomize(val, hardRange, softRange, swingRange):
	negativeComponent = swingRange.getPercentile(random.random()) * softRange.RANGE
	positiveComponent = swingRange.getPercentile(random.random()) * softRange.RANGE
	
	if val > hardRange.MAX:
		return val - negativeComponent
	elif val < hardRange.MIN:
		return val + positiveComponent
	elif val < softRange.MIN:
		return val + (1.25 * positiveComponent) - (0.5 * negativeComponent)
	elif val > softRange.MAX:
		return val + (0.5 * positiveComponent) - (1.25 * negativeComponent)
	else:
		return val + positiveComponent - negativeComponent 

def partialRandom(deviceID, messageNum, previousJSON):
	result = {}
	result[alias_device] = deviceID
	result[alias_sent] = messageNum
	result[alias_heart] = float("{0:.2f}".format(randomize(previousJSON[alias_heart], heartHardRange, heartSoftRange, heartSwingRange)))
	result[alias_blood] = float("{0:.2f}".format(randomize(previousJSON[alias_blood], bloodHardRange, bloodSoftRange, bloodSwingRange)))
	result[alias_resp] = float("{0:.2f}".format(randomize(previousJSON[alias_resp], respHardRange, respSoftRange, respSwingRange)))
	return result

class Device:
	ID = 0
	messagesSent = 0
	latestValidMessage = {}

	def __init__(self, did, heart=100, blood=75.0, resp=30, messageNum=0):
		self.ID = did
		self.messagesSent = 0
		self.latestValidMessage = {alias_device:did, alias_heart:heart, alias_resp:resp, alias_blood:blood, alias_sent:messageNum}

	def Copy(self):
		other = Device(self.ID)
		other.messagesSent = self.messagesSent
		other.latestValidMessage = self.latestValidMessage
		return other

	def NextMessage(self):
		response = partialRandom(self.ID, self.messagesSent, self.latestValidMessage)
		self.messagesSent += 1
		return response