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

def partialRandom(deviceID, messageNum, previousJSON, clean=False, garble=0.05, drop=0.1):
	result = {}
	if not clean:
		if random.random() < drop:
			return (drop_code, None)
		if random.random() < garble:
			return (garble_code, garbled())
	result[alias_device] = deviceID
	result[alias_sent] = messageNum
	result[alias_heart] = float("{0:.2f}".format(randomize(previousJSON[alias_heart], heartHardRange, heartSoftRange, heartSwingRange)))
	result[alias_blood] = float("{0:.2f}".format(randomize(previousJSON[alias_blood], bloodHardRange, bloodSoftRange, bloodSwingRange)))
	result[alias_resp] = float("{0:.2f}".format(randomize(previousJSON[alias_resp], respHardRange, respSoftRange, respSwingRange)))
	return (normal_code, result)

class Device:
	ID = 0
	messagesSent = 0
	latestValidMessage = {}
	garble = 0.05
	drop = 0.15

	def __init__(self, did, heart=100, blood=75.0, resp=30, messageNum=0):
		self.ID = did
		self.messagesSent = 0
		self.latestValidMessage = {alias_device:did, alias_heart:heart, alias_resp:resp, alias_blood:blood, alias_sent:messageNum}

	def Copy(self):
		other = Device(self.ID)
		other.messagesSent = self.messagesSent
		other.latestValidMessage = self.latestValidMessage
		other.garble = self.garble
		other.drop = self.drop
		return other

	def Copy_Settings(self, other):
		self.garble = other.garble
		self.drop = other.drop


	def NextMessage(self):
		response_code, response = partialRandom(self.ID, self.messagesSent, self.latestValidMessage, garble=self.garble, drop=self.drop)
		if response_code == normal_code:
			self.latestValidMessage = response
		self.messagesSent += 1
		return response