using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.CMD
{
    public class Parser
    {
        public List<string> Parse(string path)
        {
            List<string> list = new List<string>();
            try
            {
                using (var sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        list.Add(sr.ReadLine());
                    }
                }

                return list;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<(string, List<string>)> ParseCSV(string path)
        {
            var list = new List<(string, List<string>)>();

            try
            {
                using(var sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        var row = sr.ReadLine();
                        var columns = row.Split(';').ToList();
                        list.Add((columns[0], columns.Skip(1).ToList()));
                    }
                }

                return list;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public List<(string,string,int,string)> ParseCSVMember(string path)
        {
            var list = new List<(string, string, int, string)>();

            try
            {
                using (var sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        var row = sr.ReadLine();
                        var columns = row.Split(';').ToList();
                        list.Add((columns[0], columns[1], int.Parse(columns[2]) , columns[3]));
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
