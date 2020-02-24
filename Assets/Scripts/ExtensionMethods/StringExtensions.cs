using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

public static class StringExtensions 
{
    public static Vector3 Vector(this string data) {
        
        string[] splits = data.Split(',');
        Vector3 v = new Vector3();
        
        for (int i = 0; i < splits.Length; i++) {
            v[i] = float.Parse(splits[i]);
        }

        return v;
        
    }

    public static IEnumerable<string> SplitByCount(this string str, int chunkCount) {
        return str.SplitBySize(str.Length / chunkCount);
    }
    
    public static IEnumerable<string> SplitBySize(this string str, int chunkSize) {
        return Enumerable.Range(0, str.Length / chunkSize)
            .Select(i => str.Substring(i * chunkSize, chunkSize));
    }

    public static int NumWords(this string str) {
        return str.Split(' ').Length;
    }

    public static IEnumerable<string> SplitWholeWordsByCount(this string str, int chunkCount) {

        List<string> sentences = new List<string>();
        string[] splits = str.Split(' ');

        for (int i = 0; i < chunkCount; i++) {

            string sentence = "";

            //int startIndex = 
            for (int j = 0; j < splits.Length / chunkCount; j++) {
                sentence += splits[j] + " ";
            }

            sentences.Add(sentence);
        }

        return sentences;

    }

    public static string GenerateRandomString(int length) {
    
        string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        StringBuilder result = new StringBuilder(length);
        for (int i = 0; i < length; i++) {
            int index = UnityEngine.Random.Range(0, characters.Count()); 
            result.Append(characters[index]);
        }
        return result.ToString();
    }
    
}
