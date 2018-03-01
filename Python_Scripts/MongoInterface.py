from pymongo import MongoClient, database

client = MongoClient('localhost', 27017)
#db = client.randomdata
#collection = db.randomTest

alias_device = "did"
alias_sent = "sent"
alias_heart = "h"
alias_blood = "b"
alias_resp = "r"
alias_rcv = "rcv"

expected_fields = ["did", "sent", "h", "b", "r", "rcv"]
defaults = {k:-1 for k in expected_fields}

def get_database_and_collection(databaseName, collectionName):
	db = get_database(databaseName)
	col = get_collection(db, collectionName)
	return (db, col)

def get_database(databaseName):
	return database.Database(client, databaseName)

def get_collection(db, collectionName):
	return db[collectionName]

def drop_database(db, client=MongoClient('localhost', 27017)):
	#print(MongoClient().database_names())
	client.drop_database(db)
	#print(MongoClient().database_names())

def drop_collection(collection):
	collection.drop()

def insert_json(collection, json, verbose=False):
	if verbose:
		print("Insert: {}".format(json))
	collection.insert_one(json)

def insert_many_json(collection, jsonList):
	collection.insert_many(jsonList)

def make_default_indexes(collection):
	collection.create_index([(alias_device, 1)], background=True)
	collection.create_index([(alias_sent, -1)], background=True)
	collection.create_index([(alias_device, 1), (alias_sent, -1)], background=True)

def make_index(collection, indexFields):

	if len(collection.find({ index: { "$exists": true} })):
		collection.create_index(index)
	else:
		print("{} does not exist in database".format(index))

def search_by_device_id(collection, id, limit=10000):
	search = []
	for result in collection.find( 
		filter={alias_device:id}, 
		projection={"_id":0},
		limit=limit,
		sort=[(alias_sent,1)]
	):
		search.append(result)
	return search

def search_by_device_ids(collection, idList, limit=10000):
	search = {}
	for dID in idList:
		search[dID] = search_by_device_id(collection, dID, limit)
	return search

def search_by_time_range(collection, timeStart, timeEnd, limit=10000):
	search = {}
	for result in collection.find( 
		filter = { alias_sent:{ "$gte": timeStart, "$lte": timeEnd } },
		projection = {"_id":0},
		limit = limit,
		sort = [ (alias_device, 1), (alias_sent, -1) ]
	):
		if not search.get( result[ alias_device ] ):
			search[ result[ alias_device ] ] = []
		search[ result[ alias_device ] ] = list(result)
	return search

def search_by_time_and_device_id(id, timeStart, timeEnd, limit=10000):
	results = []

def get_collection_config(collection):
	return collection.find_one(
		filter={"config": {"$exists": True} },
		projection = {"_id":0}
		)
