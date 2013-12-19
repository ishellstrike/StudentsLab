using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace StudentsLab
{
    public static class SqlFunctions {

        //CREATE DATABASE univer;
        //USE univer;
        
        public static MySqlConnection EstablishConnection() {
            var a = new MySqlConnection(@"Server=localhost;Database=univer;Uid=root;Pwd=admin;");
            a.Open();
            return a;
        }

        public static void DropAll(MySqlConnection msc) {
            var dropStudentsTable = new MySqlCommand(
                @"DROP TABLE IF EXISTS `students`", msc);
            dropStudentsTable.ExecuteNonQuery();

            var dropLessonsTable = new MySqlCommand(
                @"DROP TABLE IF EXISTS `lessons`", msc);
            dropLessonsTable.ExecuteNonQuery();

            var dropTeachersTable = new MySqlCommand(
                @"DROP TABLE IF EXISTS `teachers`", msc);
            dropTeachersTable.ExecuteNonQuery();
        }

        public static void RecreateTables(MySqlConnection msc) {
            var createStudentsTable = new MySqlCommand(
                @"CREATE TABLE IF NOT EXISTS `students` (
                `student_id` int(11) NOT NULL AUTO_INCREMENT,
                `name` varchar(255) NOT NULL,
                `group` int(11) NOT NULL,
                PRIMARY KEY (`student_id`));", msc);
            createStudentsTable.ExecuteNonQuery();

            var createLessonsTable = new MySqlCommand(
                @"CREATE TABLE IF NOT EXISTS `lessons` (
                `lesson_id` int(11) NOT NULL AUTO_INCREMENT,
                `predmet` varchar(255) NOT NULL,
                `classroom` int(11) NOT NULL,
                `group` int(11) NOT NULL,
                `teacher_id` int(11) NOT NULL,
                `lesson_time` int(11) NOT NULL,
                PRIMARY KEY (`lesson_id`));", msc);
            createLessonsTable.ExecuteNonQuery();

            var createTeachersTable = new MySqlCommand(
                @"CREATE TABLE IF NOT EXISTS `teachers` (
                `teacher_id` int(11) NOT NULL AUTO_INCREMENT,
                `name` varchar(255) NOT NULL,
                PRIMARY KEY (`teacher_id`));", msc);
            createTeachersTable.ExecuteNonQuery();
        }

        public static void FillTestData(MySqlConnection msc)
        {
            var tempStudents = new MySqlCommand(
                @"INSERT INTO `students` (`student_id`, `name`, `group`) VALUES
                (1, 'Самсонов Андрей Федорович', 32),
                (2, 'Пономарев Александр Георгиевич', 12),
                (3, 'Емельяненко Алексей Иванович', 11),
                (4, 'Коноплев Даниил Михайлович', 12),
                (5, 'Озерова Екатерина Викторовна', 22),
                (6, 'Харченко Виктория Васильевна', 21),
                (7, 'Салфетников Игорь Вячесловович', 22),
                (8, 'Гузенко Олег Владимирович', 21),
                (9, 'Ворожейкин Никита Александрович', 32),
                (10, 'Каразия Роман Георгиевич', 31),
                (11, 'Пономарев Святослав Иванович', 32),
                (12, 'Харченко Ярополк Георгиевич', 31),
                (13, 'Емельяненко Арсений Георгиевич', 22),
                (14, 'Пономарев Геннадий Иванович', 21),
                (15, 'Харченко Джеральд Вячесловович', 41),
                (16, 'Самсонов Лука Александрович', 31);", msc);
            tempStudents.ExecuteNonQuery();

            var tempLessons = new MySqlCommand(
                @"INSERT INTO `lessons` (`lesson_id`, `predmet`, `classroom`, `group`, `teacher_id`, `lesson_time`) VALUES
                (1, 'Математика', 112, 32, 1, 1),
                (2, 'Русский язык', 311, 32, 2, 2),
                (3, 'Базы данных', 217, 32, 3, 3),
                (4, 'Математика', 212, 32, 4, 4),
                (5, 'Математика', 112, 22, 1, 2),
                (6, 'Русский язык', 311, 22, 2, 3),
                (7, 'Базы данных', 217, 22, 3, 4),
                (8, 'Математика', 212, 22, 4, 5),
                (9, 'Математика', 112, 12, 1, 1),
                (10, 'Русский язык', 311, 11, 2, 2),
                (11, 'Базы данных', 217, 12, 3, 3),
                (12, 'Математика', 212, 12, 4, 4),
                (13, 'Математика', 112, 32, 1, 5)", msc);
            tempLessons.ExecuteNonQuery();

            var tempTeachers = new MySqlCommand(
                @"INSERT INTO `teachers` (`teacher_id`, `name`) VALUES
                (1, 'Анатлолий Вассерман'),
                (3, 'Алекстандр Смирнов'),
                (4, 'Анатлолий Никитин'),
                (5, 'Никита Валерьевич'),
                (2, 'Александр Друзь')", msc);
            tempTeachers.ExecuteNonQuery();
        }

        public static string[] GetStudentsList(MySqlConnection msc) {
            var getter = new MySqlCommand(
                @"SELECT * FROM students order by student_id", msc);
            var reader = getter.ExecuteReader();
            var list = new List<string>();
            var sb = new StringBuilder();
            while (reader.Read()) {
                sb.Append(reader.GetInt32(0));
                sb.Append(") ");
                sb.Append(reader.GetString(1));
                sb.Append(", группа №");
                sb.Append(reader.GetInt32(2));
                list.Add(sb.ToString());
                sb.Clear();
            }
            reader.Close();
            return list.ToArray();
        }

        public static string[] GetTeachersList(MySqlConnection msc)
        {
            var getter = new MySqlCommand(
                @"SELECT * FROM teachers order by teacher_id", msc);
            var reader = getter.ExecuteReader();
            var list = new List<string>();
            var sb = new StringBuilder();
            while (reader.Read())
            {
                sb.Append(reader.GetInt32(0));
                sb.Append(") ");
                sb.Append(reader.GetString(1));
                list.Add(sb.ToString());
                sb.Clear();
            }
            reader.Close();
            return list.ToArray();
        }

        public static string[] SelectPersonal(int i, MySqlConnection myConnection) {
            if (i == 0) {
                return new[]{string.Empty};
            }
            var list = new List<string>();

            var group_getter = new MySqlCommand(string.Format("SELECT `group` from students WHERE `student_id` = {0}", i), myConnection);
            var group = (Int32)group_getter.ExecuteScalar();

            var p_getter = new MySqlCommand(string.Format("SELECT * from lessons WHERE `group` = {0} ORDER BY `lesson_time` ASC", group), myConnection);
            var reader = p_getter.ExecuteReader();
            var sb = new StringBuilder();
            while (reader.Read())
            {
                sb.Append(reader.GetInt32(5));
                sb.Append("-ая пара : ");
                sb.Append(reader.GetString(1));
                sb.Append(", аудитория ");
                sb.Append(reader.GetInt32(2));
                list.Add(sb.ToString());
                sb.Clear();
            }
            reader.Close();
            return list.ToArray();
        }

        public static void CloseConnection(MySqlConnection myConnection) {
            if (myConnection != null && myConnection.State == ConnectionState.Open)
            {
                myConnection.Close();
            }
        }

        public static string[] SelectWork(int i, MySqlConnection myConnection) {
            if (i == 0)
            {
                return new[] { string.Empty };
            }
            var list = new List<string>();

            var p_getter = new MySqlCommand(string.Format("SELECT * from lessons WHERE `teacher_id` = {0} ORDER BY `lesson_time` ASC", i), myConnection);
            var reader = p_getter.ExecuteReader();
            var sb = new StringBuilder();
            while (reader.Read())
            {
                sb.Append(reader.GetInt32(5));
                sb.Append("-ая пара : ");
                sb.Append(reader.GetString(1));
                sb.Append(", аудитория ");
                sb.Append(reader.GetInt32(2));
                sb.Append(", занимается группа ");
                sb.Append(reader.GetInt32(3));
                list.Add(sb.ToString());
                sb.Clear();
            }
            reader.Close();
            return list.ToArray();
        }
    }
}
