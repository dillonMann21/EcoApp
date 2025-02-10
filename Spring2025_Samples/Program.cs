using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Xml.Serialization;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Amazon!");
            Console.WriteLine("Select Option 1 to Add to Store Inventory, or 2 to Add to Personal Cart");

            char Aoption;
            do
            {
                string? inp = Console.ReadLine();
                Aoption = inp[0];

                switch (Aoption)
                {
                    case '1':
                    case 'O':
                    case 'o':
                        // if adding to inventory
                        Console.WriteLine("C. Create new inventory item");
                        Console.WriteLine("R. Read all inventory items");
                        Console.WriteLine("U. Update an inventory item");
                        Console.WriteLine("D. Delete an inventory item");
                        // quit to break
                        Console.WriteLine("Q. Quit");

                        List<Product?> list = ProductServiceProxy.Current.Products;
                        List<Product?> Cart = new List<Product?>();
                        char choice;

                        do
                        {
                            string? input = Console.ReadLine();
                            choice = input[0];

                            switch (choice)
                            {
                                case 'C':
                                case 'c':
                                    ProductServiceProxy.Current.AddOrUpdate(new Product
                                    {
                                        Console.WriteLine("What is the product ID of this item?");
                                        Id = Console.ReadLine();
                                        Console.WriteLine("How many of this product are there?");
                                        Quantities = Console.ReadLine();
                                        Console.WriteLine("What is the name of this product?");
                                        Name = Console.ReadLine();
                                        Console.WriteLine("What is the price of this item?");
                                        Price = Console.ReadLine();
                                    });
                                    break;

                                case 'R':
                                case 'r':
                                    list.ForEach(Console.WriteLine);
                                    break;

                                case 'U':
                                case 'u':
                                    //select one of the products
                                    Console.WriteLine("Which product would you like to update?");
                                    int selection = int.Parse(Console.ReadLine() ?? "-1");
                                    var selectedProd = list.FirstOrDefault(p => p.Id == selection);

                                    if (selectedProd != null)
                                    {
                                        selectedProd.Name = Console.ReadLine() ?? "ERROR";
                                        ProductServiceProxy.Current.AddOrUpdate(selectedProd);

                                        Console.WriteLine("What is the new quantity of this item?");
                                        selectedProd.Quantities = Console.ReadLine();
                                    }
                                    break;

                                case 'D':
                                case 'd':
                                    //select one of the products and delete it
                                    Console.WriteLine("Which product would you like to update?");
                                    selection = int.Parse(Console.ReadLine() ?? "-1");
                                    ProductServiceProxy.Current.Delete(selection);
                                    Console.WriteLine("Product deleted.");
                                    break;

                                case 'Q':
                                case 'q':
                                    break;

                                default:
                                    Console.WriteLine("Error: Unknown Command");
                                    break;
                            }
                        } while (choice != 'Q' && choice != 'q');
                        break;

                    case '2':
                        Console.WriteLine("F. Finish cart and check out");
                        Console.WriteLine("A. Add items to cart from store inventory");
                        Console.WriteLine("T. Return items to inventory, delete cart");
                        Console.WriteLine("S. Show all items in cart");
                        Console.WriteLine("Q. Quit");

                        char cartChoice;
                        do
                        {
                            string? cartInput = Console.ReadLine();
                            cartChoice = cartInput[0];

                            switch (cartChoice)
                            {
                                case 'F':
                                case 'f':
                                    Console.WriteLine("Your total will be: $");
                                    var total = 0;
                                    Cart.Select(p => p.Price).ForEach(p => total += p);
                                    total += (total * 0.07);
                                    Console.WriteLine(total);
                                    break;

                                case 'A':
                                case 'a':
                                    //create product in cart
                                    Console.WriteLine("Enter the item/product ID you would like to add to cart");
                                    Product cartitem = new Product();
                                    cartitem.Id = Console.ReadLine();

                                    if (list.GetAmount(cartitem) > 0)
                                    {
                                        //check quantities of product
                                        Console.WriteLine("How many of this item would you like to add to cart?");
                                        int localnum = int.Parse(Console.ReadLine() ?? "0");

                                        // item found with valid stock
                                        if (list.GetAmount(cartitem) >= localnum)
                                        {
                                            for (int i = 0; i < localnum; i++)
                                            {
                                                Cart.Add(cartitem);
                                                list.Remove(cartitem); // Corrected method from Delete to Remove
                                            }
                                            Console.WriteLine("Product added to cart.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Not enough product in inventory, please enter a lower number.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Item does not exist in inventory, did you enter the correct product ID?");
                                    }
                                    break;

                                case 'T':
                                case 't':
                                    Console.WriteLine("Trashing cart, returning all items to inventory");
                                    foreach (var productVariable in Cart)
                                    {
                                        list.Add(productVariable);  // Corrected method from AddOrUpdate to Add
                                    }
                                    Cart.Clear();  // To clear cart after returning items
                                    break;

                                case 'S':
                                case 's':
                                    Console.WriteLine("Items in cart:");
                                    Cart.ForEach(Console.WriteLine);
                                    break;

                                case 'Q':
                                case 'q':
                                    break;

                                default:
                                    Console.WriteLine("Error: Unknown Command");
                                    break;
                            }
                        } while (cartChoice != 'Q' && cartChoice != 'q');
                        break;

                    default:
                        Console.WriteLine("Error: Unknown Option");
                        break;
                }
            } while (Aoption != 'Q' && Aoption != 'q');
        }
    }
}
