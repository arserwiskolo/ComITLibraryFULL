using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WordGuess.Models;

namespace mvcsample.Controllers
{
    // public class Word{
    //     public string Value {get; set;}
    //     public string Category{get; set;}
    // }
    public class WordController : Controller
    {
        
        public WordController()
        {
           
        }

        public IActionResult Index()
        {
              List<Word> myWords = new List<Word>();
                myWords.Add(new Word(){
                    Category = "People name",
                    Value = "Amanda"

                });

                myWords.Add(new Word(){
                Category = "People name",
                Value = "Anakonda"

                });

                myWords.Add(new Word(){
                Category = "People name",
                Value = "Adam"

                });

            var myView = View(myWords);
            return myView;
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }
    }
}
