/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 14.01.2014
 * Time: 12:17
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace Bargool.Acad.Library
{
    /// <summary>
    /// Вспомогательный класс для переключения текущей базы данных. Работает по шаблону "посетитель".
    /// При этом есть 2 варианта работы: открытие базы данных, не загруженной в автокад, либо переключение на уже отрытую
    /// </summary>
    public class DatabaseWorker : IDisposable
    {
        string path;
        Database previousDatabase;
        bool isAlreadyOpened;
        bool doSave;

        Database currentDb = null;
        DocumentLock docLock = null;

        public Database WorkingDatabase
        {
            get { return currentDb; }
        }

        private DatabaseWorker()
        {
            previousDatabase = HostApplicationServices.WorkingDatabase;
        }

        /// <summary>
        /// Конструктор посетителя. Если указанный документ не открыт - будет открыта его БД
        /// </summary>
        /// <param name="path">Путь к обрабатываемой БД</param>
        public DatabaseWorker(string path)
            : this()
        {
            this.path = path;
            OpenDatabase();
        }

        /// <summary>
        /// Конструктор посетителя. Если указанная БД не загружена - будет открыта
        /// </summary>
        /// <param name="workDatabase">Обрабатываемая БД</param>
        public DatabaseWorker(Database workDatabase)
            : this()
        {
            this.path = workDatabase.OriginalFileName;
            OpenDatabase();
        }

        private void OpenDatabase()
        {
            Document alreadyOpenedDocument =
                Application
                .DocumentManager
                .Cast<Document>()
                .FirstOrDefault(d => d.Name.Equals(this.path, StringComparison.InvariantCultureIgnoreCase));
            this.isAlreadyOpened = alreadyOpenedDocument != null;
            currentDb = null;
            docLock = null;
            // Если искомый файл уже открыт, то надо не открывать БД, а блокировать документ и обрабатывать
            if (isAlreadyOpened)
            {
                docLock = alreadyOpenedDocument.LockDocument();
                HostApplicationServices.WorkingDatabase = alreadyOpenedDocument.Database;
                currentDb = alreadyOpenedDocument.Database;
            }
            else
            {
                if (!System.IO.File.Exists(this.path))
                    throw new System.IO.FileNotFoundException(this.path);

                try
                {
                    currentDb = new Database(false, true);
                    currentDb.ReadDwgFile(this.path, System.IO.FileShare.ReadWrite, true, null);
                    currentDb.CloseInput(true);
                }
                catch (System.Exception ex)
                {
                    string message = "При открытии файла " + this.path + " возникла ошибка: ";
                    ex.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ex, message + ex.Message);
                    throw ex;
                }
            }
        }

        private void CloseDatabase()
        {
            if (isAlreadyOpened)
            {
                if (docLock != null)
                    docLock.Dispose();
            }
            else
            {
                if (currentDb != null)
                {
                    string tempPath = Path.GetTempFileName();
                    if (doSave)
                        currentDb.SaveAs(tempPath, DwgVersion.Current);
                    currentDb.Dispose();

                    if (doSave)
                    {
                        File.Delete(this.path);
                        File.Move(tempPath, this.path);
                    }
                }
            }
        }

        /// <summary>
        /// Запуск обработки "посещения". База при этом не сохраняется
        /// </summary>
        /// <param name="visitedElement">Элемент - посещаемый объект</param>
        public void Visit(IDatabaseVisitor visitedElement)
        {
            Visit(visitedElement, false);
        }

        /// <summary>
        /// Запуск обработки "посещения"
        /// </summary>
        /// <param name="visitedElement">Элемент - посещаемый объект</param>
        /// <param name="doSave">Сохранять ли БД (если обрабатывается уже открытый документ - БД сохраняться не будет)</param>
        public void Visit(IDatabaseVisitor visitedElement, bool doSave)
        {
            this.doSave = doSave;
            try
            {
                visitedElement.Accept(currentDb);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            CloseDatabase();
            HostApplicationServices.WorkingDatabase = previousDatabase;
        }
    }
}
