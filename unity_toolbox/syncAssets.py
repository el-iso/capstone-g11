from tkinter import Tk
from tkinter.filedialog import askopenfilenames, askdirectory, asksaveasfilename
from tkinter import messagebox

import os
import sys
import shutil

types = (
	("All Files", "*"),
	("Scripts","*.cs"),
	("Materials","*.mat"),
	)

def directoryInfo(path, depth=0):
	basename = os.path.split(path)[1]
	dirsResult = []
	filesResult = []
	fileTypes = {}
	for childPath, dirs, files in os.walk(path):
		dirsResult += dirs
		filesResult += [os.path.join(childPath,file) for file in files]
		for file in files:
			extension = file.split(".")[1]
			fileTypes[extension] = fileTypes.get(extension, 0) + 1
		print("{} Items in {}\t{} Dir(s), {} File(s)".format(len(dirs+files), childPath.replace(path,basename), len(dirs), len(files)))
	return (dirsResult, filesResult, fileTypes)

def extensionDictionary(fileList):
	result = {}
	for file in fileList:
		try:
			extension = file.split(".")[1]
		except:
			continue
		result[extension] = result.get(extension,[]) + [file]
	return result

def writeSyncList(path, sourceTargetDict):
	with open(path, "w") as saveFile:
		for source, target in sourceTargetDict.items():
			saveFile.write("{}\n{}\n\n".format(source, target))

	with open(os.path.join(os.path.split(path)[0], ".gitignore"), "a") as ignoreFile:
		ignoreFile.write("sync.txt")


def initSync(fileList):
	syncNum = 0
	typeDict = extensionDictionary(fileList)
	targetDict = {}
	targetPath = askdirectory(initialdir="./Projects", title="Choose Target Assets Folder")

	importDirPath = os.path.join(targetPath, "Imports")
	if not os.path.exists(importDirPath):
		os.mkdir(importDirPath)
		print(importDirPath)
	
	for extension,files in typeDict.items():
		extensionDirPath = os.path.join(importDirPath,extension.upper())
		if not os.path.exists(extensionDirPath):
			os.mkdir(extensionDirPath)
			print(extensionDirPath)
	
		for file in files:
			targetDirPath = os.path.join(extensionDirPath,os.path.basename(file))
			targetDict[file] = targetDirPath
			try:
				shutil.copyfile(file, targetDirPath)
				syncNum += 1
			except Exception as e:
				print("{}File Copy Failed\n{}".format(file, str(e)))
	print("Synchronized: {} File(s)".format(syncNum))

	writeSyncList(os.path.join(targetPath, "sync.txt"), targetDict)




def askMakeNewSyncList():
	Tk().withdraw()

	sourceDirectory = askdirectory(initialdir="./Assets", title="Choose Assets directory to import")
	dirs, files, fileTypes = directoryInfo(sourceDirectory)
	
	askMessage = "{} File(s) in {} Sub-Directories, Import All?".format(len(files), len(dirs))
	for k,v in fileTypes.items():
		askMessage += "\n{}\t{}".format(k,v)
	
	if messagebox.askyesno("Import", askMessage):
		syncMap = initSync(files)
	#else:




askMakeNewSyncList()


#Tk().withdraw()
#tdpPath = askopenfilenames(initialdir=".", title="Choose TDP file to simulate", filetypes=types)