using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public sealed class Task15 : BaseTask
    {
        /*
        
        --- Day 15: Science for Hungry People ---
        Today, you set out on the task of perfecting your milk-dunking cookie recipe.
        All you have to do is find the right balance of ingredients.

        Your recipe leaves room for exactly 100 teaspoons of ingredients.
        You make a list of the remaining ingredients you could use to finish the recipe (your puzzle input) and their properties per teaspoon:
            - capacity (how well it helps the cookie absorb milk)
            - durability (how well it keeps the cookie intact when full of milk)
            - flavor (how tasty it makes the cookie)
            - texture (how it improves the feel of the cookie)
            - calories (how many calories it adds to the cookie)

        You can only measure ingredients in whole-teaspoon amounts accurately, and you have to be accurate so you can reproduce your results in the future.
        The total score of a cookie can be found by adding up each of the properties (negative totals become 0) and then multiplying together everything except calories.

        For instance, suppose you have these two ingredients:
            Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
            Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3

        Then, choosing to use 44 teaspoons of butterscotch and 56 teaspoons of cinnamon (because the amounts of each ingredient must add up to 100) would result in a cookie with the following properties:
            - A capacity of 44*-1 + 56*2 = 68
            - A durability of 44*-2 + 56*3 = 80
            - A flavor of 44*6 + 56*-2 = 152
            - A texture of 44*3 + 56*-1 = 76

        Multiplying these together (68 * 80 * 152 * 76, ignoring calories for now) results in a total score of 62842880, which happens to be the best score possible given these ingredients.
        If any properties had produced a negative total, it would have instead become zero, causing the whole score to multiply to zero.

        Task: Given the ingredients in your kitchen and their properties, what is the total score of the highest-scoring cookie you can make?
        Solution: 

        */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input15.txt");
            List<Ingredient> ingredients = ConvertToIngredients(input);

            int numberOfTeaspoons = 100;

            return base.Part1();
        }

        public override string Part2()
        {
            return base.Part2();
        }

        private List<Ingredient> ConvertToIngredients(List<string> ingredientsInfo)
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            for(int i = 0; i < ingredientsInfo.Count; i++)
            {
                int indexOfNameEnd = ingredientsInfo[i].IndexOf(':');
                string name = ingredientsInfo[i][..indexOfNameEnd];

                string[] propertyValues = ingredientsInfo[i][(indexOfNameEnd + 2)..].Split(',');
                int[] values = new int[propertyValues.Length];

                for (int j = 0; j < propertyValues.Length; j++)
                {
                    propertyValues[j] = propertyValues[j].Trim();
                    int indexOfSpace = propertyValues[j].IndexOf(' ');
                    string value = propertyValues[j].Substring(indexOfSpace + 1);
                    values[j] = int.Parse(value);
                }

                ingredients.Add(new Ingredient(name, values[0], values[1], values[2], values[3], values[4]));
            }

            return ingredients;
        }


        private sealed class Ingredient
        {
            public Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
            {
                Name = name;
                Capacity = capacity;
                Durability = durability;
                Flavor = flavor;
                Texture = texture;
                Calories = calories;
            }

            private int _quantity = 1;

            public string Name { get; }

            public int Capacity { get; }

            public int Durability { get; }

            public int Flavor { get; }

            public int Texture { get; }

            public int Calories { get; }

            public int Quantity
            {
                get
                {
                    return _quantity;
                }
                set
                {
                    if(value < 1)
                    {
                        throw new ArgumentOutOfRangeException("value");
                    }

                    _quantity = value;
                }
            }
        }

        private sealed class Recipe
        {
            //public Recipe(int numberOfTeaspoons)
            //{
            //    _numberOfTeaspoons = numberOfTeaspoons;
            //}

            //private int _numberOfTeaspoons;
            //private List<Ingredient> _ingredients = new List<Ingredient>();

            public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

            public override string ToString()
            {
                string returnValue = string.Empty;

                foreach(Ingredient ingredient in Ingredients)
                {
                    returnValue += $"{ingredient.Name}{ingredient.Quantity}";
                }

                return returnValue;
            }
        }
    }
}
