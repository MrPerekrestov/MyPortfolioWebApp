using MyPortfolioWebApp.DatabaseManager.DatabaseService.ProjectViewerReturnTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyPortfolioWebApp.DatabaseManager.DatabaseService
{
    public static class ProjectViewer
    {
        public static IEnumerable<ProjectInfo> GetProjectsInfo(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var command = new MySqlCommand("", connection);
            try
            {
                command.CommandText = "SELECT * FROM `projects`";

                connection.Open();

                var reader = command.ExecuteReader();
                var projectsInfoDataTable = new DataTable();
                projectsInfoDataTable.Load(reader);

                foreach (DataRow row in projectsInfoDataTable.Rows)
                {
                    var projectInfo = new ProjectInfo
                    {
                        Id = (int)row[0],
                        Name = (string)row[1],
                        Created = DateTime.Parse(row[2].ToString()),
                        Published = DateTime.Parse(row[3].ToString()),
                        Description = (string)row[4],
                        ImagesChanged = DateTime.Parse(row[5].ToString())
                    };
                    yield return projectInfo;
                }
            }
            finally
            {
                connection?.Close();
                command?.Dispose();
            }

        }
        public static ProjectInfo GetProjectInfo(int projectId, string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var command = new MySqlCommand("", connection);
            try
            {
                command.CommandText = $"SELECT * FROM `projects` WHERE `id` = {projectId} ";

                connection.Open();

                var reader = command.ExecuteReader();
                var projectsInfoDataTable = new DataTable();
                projectsInfoDataTable.Load(reader);
                var row = projectsInfoDataTable.Rows[0];
                var projectInfo = new ProjectInfo
                {
                    Id = (int)row[0],
                    Name = (string)row[1],
                    Created = DateTime.Parse(row[2].ToString()),
                    Published = DateTime.Parse(row[3].ToString()),
                    Description = (string)row[4],
                    ImagesChanged = DateTime.Parse(row[5].ToString())
                };
                return projectInfo;
            }
            finally
            {
                connection?.Close();
                command?.Dispose();
            }

        }
        public static ProjectInfo GetProjectInfo(string projectName, string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var command = new MySqlCommand("", connection);
            try
            {
                connection.Open();
                command.CommandText = $"SELECT `id` FROM `projects` WHERE `name` = '{projectName}';";
                var projectId = command.ExecuteScalar();

                command.CommandText = $"SELECT * FROM `projects` WHERE `id` = {projectId} ";
                var reader = command.ExecuteReader();

                var projectsInfoDataTable = new DataTable();
                projectsInfoDataTable.Load(reader);
                var row = projectsInfoDataTable.Rows[0];
                var projectInfo = new ProjectInfo
                {
                    Id = (int)row[0],
                    Name = (string)row[1],
                    Created = DateTime.Parse(row[2].ToString()),
                    Published = DateTime.Parse(row[3].ToString()),
                    Description = (string)row[4]
                };
                return projectInfo;
            }
            finally
            {
                connection?.Close();
                command?.Dispose();
            }

        }
        public static IEnumerable<ImageInfo> GetImagesInfo(string projectName, string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var command = new MySqlCommand("", connection);
            try
            {
                connection.Open();
                command.CommandText = $"SELECT `id` FROM `projects` WHERE `name` = '{projectName}';";
                var projectId = command.ExecuteScalar();

                command.CommandText = $"SELECT * FROM `projects_images` WHERE `projectid` = {projectId};";
                var reader = command.ExecuteReader();
                var imageInfoDataTable = new DataTable();
                imageInfoDataTable.Load(reader);

                foreach (DataRow row in imageInfoDataTable.Rows)
                {
                    var imageInfo = new ImageInfo
                    {
                        Id = (int)row[1],
                        Extension = (string)row[3],
                        TimeStamp = DateTime.Parse(row[4].ToString())
                    };
                    yield return imageInfo;
                }
            }
            finally
            {
                connection?.Close();
                command?.Dispose();
            }
        }
        public static IEnumerable<ImageInfo> GetImagesInfo(int projectId, string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var command = new MySqlCommand("", connection);
            try
            {
                connection.Open();

                command.CommandText = $"SELECT * FROM `projects_images` WHERE `projectid` = {projectId};";
                var reader = command.ExecuteReader();
                var imageInfoDataTable = new DataTable();
                imageInfoDataTable.Load(reader);

                foreach (DataRow row in imageInfoDataTable.Rows)
                {
                    var imageInfo = new ImageInfo
                    {
                        Id = (int)row[1],
                        Extension = (string)row[3],
                        TimeStamp = DateTime.Parse(row[4].ToString())
                    };
                    yield return imageInfo;
                }
            }
            finally
            {
                connection?.Close();
                command?.Dispose();
            }
        }
        public static byte[] GetImage(int projectId, int imageId, string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var command = new MySqlCommand("", connection);
            try
            {
                command.CommandText = $"SELECT `image` FROM `projects_images` " +
                                      $"WHERE `projectid` = {projectId} AND `imageid` = {imageId}";
                connection.Open();
                var result = (byte[])command.ExecuteScalar();

                return result;
            }
            finally
            {
                connection?.Close();
                command?.Dispose();
            }
        }        
      
        public static string GetHtml(int projectId, string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var command = new MySqlCommand("", connection);
            try
            {
                command.CommandText = $"SELECT `html` FROM `projects_html` " +
                                      $"WHERE `id` = {projectId}";
                connection.Open();
                var result = (string)command.ExecuteScalar();
                return result;
            }
            finally
            {
                connection?.Close();
                command?.Dispose();
            }
        }
        public static DateTime GetHtmlTimeStamp(int projectId, string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var command = new MySqlCommand("", connection);
            try
            {
                command.CommandText = $"SELECT `htmltimestamp` FROM `projects_html` " +
                                      $"WHERE `id` = {projectId}";
                connection.Open();
                var result = (DateTime)command.ExecuteScalar();
                return result;
            }
            finally
            {
                connection?.Close();
                command?.Dispose();
            }
        }
        public static bool CheckIfProjectExists(string projectName, string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var command = new MySqlCommand("", connection);
            try
            {
                connection.Open();                
                command.CommandText = $"SELECT count(*) FROM `projects` WHERE `name` = '{projectName}';";
                var result = (long)command.ExecuteScalar();
                return result == 1;
            }
            finally
            {
                connection?.Close();
                command?.Dispose();
            }

        }
        public static bool CheckIfProjectExists(int projectId, string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            var command = new MySqlCommand("", connection);
            try
            {
                connection.Open();
                command.CommandText = $"SELECT count(*) FROM `projects` WHERE `id` = '{projectId}';";
                var result = (long)command.ExecuteScalar();
                return result == 1;
            }
            finally
            {
                connection?.Close();
                command?.Dispose();
            }
        }
    }
}
