from tkinter import Tk
from tkinter.filedialog import askopenfilenames, askdirectory, asksaveasfilename
from tkinter import messagebox

import os
import sys

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

def writeSyncList(fileList):
	for file in fileList:
		print(file)

def askMakeNewSyncList():
	Tk().withdraw()

	sourceDirectory = askdirectory(initialdir="./Assets", title="Choose Assets directory to import")
	dirs, files, fileTypes = directoryInfo(sourceDirectory)
	
	askMessage = "{} File(s) in {} Sub-Directories, Import All?".format(len(files), len(dirs))
	for k,v in fileTypes.items():
		askMessage += "\n{}\t{}".format(k,v)
	
	if messagebox.askyesno("Import", askMessage):
		writeSyncList(files)
	#else:




askMakeNewSyncList()


#Tk().withdraw()
#tdpPath = askopenfilenames(initialdir=".", title="Choose TDP file to simulate", filetypes=types)