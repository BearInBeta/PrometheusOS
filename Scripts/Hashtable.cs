using System;
using System.Collections.Generic;


public class Hashtable
{
    private List<string>[] table = new List<string>[10007];

    private int size;

    private int HFunction(string s)
    {
        int num = 0;
        for (int i = 0; i < s.ToLower().Length; i++)
        {
            num += (int)s.ToLower()[i];
        }
        return num % this.table.Length;

    }

    public void read(string[] words)
    {
        for (int i = 0; i < words.Length; i++)
        {
            string s = words[i];
            this.put(s);
        }

    }

    public void put(string s)
    {
        int num = this.HFunction(s.ToLower());
        if (this.get(s.ToLower()) == null)
        {
            if (this.table[num] == null)
            {
                this.table[num] = new List<string>();
            }
            this.table[num].Add(s.ToLower());
            this.size++;
        }

    }

    public string get(string s)
    {
        int num = this.HFunction(s.ToLower());
        if (this.table[num] == null)
        {
            return null;
        }
        if (this.table[num].Contains(s.ToLower()))
        {
            return s.ToLower();
        }
        return null;

    }

    public void remove(string s)
    {
        int num = this.HFunction(s.ToLower());
        if (this.table[num] == null)
        {
            Console.Write("The word you entered isn't in the table");
            return;
        }
        this.table[num].Remove(s.ToLower());
        this.size--;

    }

    public List<string> closestVlas(string s, int max)
    {
        int num = this.HFunction(s.ToLower());
        List<string> list = new List<string>();
        if (this.table[num] != null)
        {
            foreach (string current in this.table[num])
            {
                list.Add(current);
                if (list.Count >= max)
                {
                    List<string> result = list;
                    return result;
                }
            }
        }
        List<string>[] array = this.table;
        for (int i = 0; i < array.Length; i++)
        {
            List<string> list2 = array[i];
            if (list2 != this.table[num] && list2 != null)
            {
                foreach (string current2 in list2)
                {
                    list.Add(current2);
                    if (list.Count >= max)
                    {
                        List<string> result = list;
                        return result;
                    }
                }
            }
        }
        return list;

    }

    public int getSize()
    {
        return this.size;
    }
}
