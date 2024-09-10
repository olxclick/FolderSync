# Folder Synchronizer

## Overview

This project implements a folder synchronization tool using **C#**. The purpose of the tool is to keep a full, identical copy of a `source` folder in a `replica` folder by periodically synchronizing them. It logs file creation, copying, and deletion actions both to the console and a log file. The synchronization is one-way, meaning the `replica` folder is updated to match the `source` folder.

## Features

- **One-way synchronization**: Ensures that the `replica` folder is a complete and up-to-date copy of the `source` folder.
- **File operations logging**: Logs every file creation, copying, and deletion event.
- **Configurable via command-line arguments**: Allows customization of folder paths, synchronization interval, and log file path via the command-line.
- **Periodic Synchronization**: Automatically performs synchronization at regular intervals specified by the user.
- **Console and file logging**: Logs synchronization actions both to the console and to a file for easy auditing.

## Usage

### Command-Line Arguments

The program takes the following command-line arguments:

1. **Source Folder Path**: The directory path for the folder you want to sync from.
2. **Replica Folder Path**: The directory path for the folder you want to sync to.
3. **Synchronization Interval**: The time interval (in seconds) between synchronization checks.
4. **Log File Path**: The path to the file where all file operations will be logged.

### Example

```bash
dotnet run "/path/to/source" "/path/to/replica" interval in seconds "/path/to/logfile.log"
