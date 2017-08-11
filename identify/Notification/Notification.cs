using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Timers;
using identify.Logs;
using Newtonsoft.Json;

namespace identify.Notification
{
    class Notification
    {
        IdentifyEntities db = new IdentifyEntities();
        public Notification()
        {
            var notificationTimer = new System.Timers.Timer(60000);  // [900000 ms = 15 minutes]   [3600000 ms = 1 hour]  [86400000 ms = 24 hours]
            notificationTimer.Elapsed += new ElapsedEventHandler(OnNotification);   
            notificationTimer.Enabled = true;
        }

        private void OnNotification(object sender, ElapsedEventArgs e)
        {
            // Seems like 650 lines of JSON events is the max that will transfer successfully to SQL as Json data of varchar(MAX)
            try
            {
                string extension = Properties.Settings.Default.OutputExtension;
                var filesToCollect = Directory.GetFiles(Properties.Settings.Default.OutputFilePath).Where(x => Path.GetExtension(x).Contains(extension));
                foreach (var file in filesToCollect)
                {
                    var LogItem = GatherLogData(file);
                    UploadLogDataToCentralServer(LogItem);
                    ArchiveFile(file);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Logging.LogEntry(ex);
            }
        }

        private void ArchiveFile(string file)
        {
            try
            {
                var currentFilePath = Path.GetDirectoryName(file);
                var currentFileName = Path.GetFileNameWithoutExtension(file);
                var currentFileExtension = Path.GetExtension(file);
                string ArchiveFolderName = "ARCHIVE";
                DateTime timeStamp = DateTime.Now;
                string subArchiveFolder = String.Format("{0}.{1}.{2}.{3}.{4}", timeStamp.Month, timeStamp.Day, timeStamp.Year, timeStamp.Hour, timeStamp.Minute);
                var archiveFolderPath = Path.Combine(currentFilePath, ArchiveFolderName, subArchiveFolder);

                if (!Directory.Exists(archiveFolderPath))
                {
                    Directory.CreateDirectory(archiveFolderPath);
                }

                var archivedFilePath = Path.Combine(archiveFolderPath, currentFileName + ".arc");
                File.Move(file, archivedFilePath);
            }
            catch (Exception ex)
            {
                Logging.LogEntry(ex);
            }
        }

        private void UploadLogDataToCentralServer(DataLog logToAdd)
        {
            try
            {
                if (logToAdd.Data != String.Empty)
                {
                    db.DataLogs.Add(logToAdd);
                }

            }
            catch (Exception ex)
            {
                Logging.LogEntry(ex);
            }
        }

        public DataLog GatherLogData(string filePath)
        {
            //TODO:  Centralize the folder for the documents, and then iterate through all of them here (not just a single one)
            var eventItem = GetFileData(filePath);
            var jsonData = JsonConvert.SerializeObject(eventItem);

            var fileType = Path.GetFileNameWithoutExtension(filePath);

            if (!currentUserExistsInDatabase(LogConstants.userAcct))
            {
                db.Users.Add(new User()
                             {
                                 Description = LogConstants.userAcct.Name
                             });

                db.SaveChanges();
            }

            DataLog newEntry = new DataLog()
            {
                DataType = db.DataTypes.FirstOrDefault(dt => dt.DataItemType == fileType),
                User = db.Users.FirstOrDefault(u => u.Description == LogConstants.userAcct.Name),
                Data = jsonData.ToString(),
                InputTime = DateTime.UtcNow
            };

            return newEntry;
        }

        private bool currentUserExistsInDatabase(WindowsIdentity userIdentity)
        {
            if (db.Users.FirstOrDefault(u => u.Description == LogConstants.userAcct.Name) == null)
            {
                return false;
            }
            return true;
        }

        private IEnumerable<Event> GetFileData(string filePath)
        {
            List<Event> streamData = new List<Event>();
            using (StreamReader data = new StreamReader(filePath))
            {
                string dataLine;
                while ((dataLine = data.ReadLine()) != null)
                {
                    Event eventDetails = dataLine.ParseLogDetails();
                    streamData.Add(eventDetails);
                }
            }
            
            return streamData;
        }
    }
}
