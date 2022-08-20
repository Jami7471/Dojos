using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    /*
    
    --- Day 12: JSAbacusFramework.io ---
    Santa's Accounting-Elves need help balancing the books after a recent order. 
    Unfortunately, their accounting software uses a peculiar storage format. That's where you come in.

    They have a JSON document which contains a variety of things: arrays ([1,2,3]), objects ({"a":1, "b":2}), numbers, and strings. 
    Your first job is to simply find all of the numbers throughout the document and add them together.

    For example:
        - [1,2,3] and {"a":2,"b":4} both have a sum of 6.
        - [[[3]]] and {"a":{"b":4},"c":-1} both have a sum of 3.
        - {"a":[-1,1]} and [-1,{"a":1}] both have a sum of 0.
        - [] and {} both have a sum of 0.

    You will not encounter any strings containing numbers.

    Task: What is the sum of all numbers in the document?
    Solution: 191164

    --- Part Two ---
    Uh oh - the Accounting-Elves have realized that they double-counted everything red.

    Ignore any object (and all of its children) which has any property with the value "red". 
    Do this only for objects ({...}), not arrays ([...]).

        - [1,2,3] still has a sum of 6.
        - [1,{"c":"red","b":2},3] now has a sum of 4, because the middle object is ignored.
        - {"d":"red","e":[1,2,3,4],"f":5} now has a sum of 0, because the entire structure is ignored.
        - [1,"red",5] has a sum of 6, because "red" in an array has no effect.

    Task: What is the sum of all numbers in the document?
    Solution: 87842

    */
    public sealed class Task12 : BaseTask
    {
        public override string Part1()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input12.txt");
            JsonDocument jsonDocument = JsonDocument.Parse(input);
            return GetSumOfDigits(jsonDocument.RootElement.EnumerateArray()).ToString();
        }

        public override string Part2()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input12.txt");
            JsonDocument jsonDocument = JsonDocument.Parse(input);
            return GetSumOfDigits(jsonDocument.RootElement.EnumerateArray(), "red").ToString();
        }

        private int GetSumOfDigits(JsonElement.ArrayEnumerator arrayEnumerator, string ignoreObjectColor = "")
        {
            int sumOfDigits = 0;

            foreach (JsonElement element in arrayEnumerator)
            {
                switch (element.ValueKind)
                {
                    case JsonValueKind.Array:
                        sumOfDigits += GetSumOfDigits(element.EnumerateArray(), ignoreObjectColor);
                        break;
                    case JsonValueKind.Number:                  
                        sumOfDigits += element.GetInt32();
                        break;
                    case JsonValueKind.Object:
                        sumOfDigits += GetSumOfDigits(element.EnumerateObject(), ignoreObjectColor);
                        break;
                    default:
                        break;
                }
            }   
            
            return sumOfDigits;
        }

        private int GetSumOfDigits(JsonElement.ObjectEnumerator objectEnumerator, string ignoreObjectColor = "")
        {
            int sumOfDigits = 0;

            foreach (JsonProperty element in objectEnumerator)
            {
                switch (element.Value.ValueKind)
                {
                    case JsonValueKind.Array:
                        sumOfDigits += GetSumOfDigits(element.Value.EnumerateArray(), ignoreObjectColor);
                        break;
                    case JsonValueKind.String:
                        if(element.Value.GetString() == ignoreObjectColor)
                        {
                            return 0;
                        }
                        break;
                    case JsonValueKind.Number:
                        sumOfDigits += element.Value.GetInt32();
                        break;
                    case JsonValueKind.Object:
                        sumOfDigits += GetSumOfDigits(element.Value.EnumerateObject(), ignoreObjectColor);
                        break;
                    default:
                        break;
                }
            }

            return sumOfDigits;
        }
    }
}
