using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WordGuess.Models;

namespace ComITLibraryMVC.Controllers
{
    public  class WordController : Controller
    {
        private static WordGuess.WordGuessSystem _library;
        
        public WordController(WordGuess.WordGuessSystem library)
        {
           _library = library;
        }

        public IActionResult Index()
        {
            var words = _library.GetAllWords();
            return View(words);
        }

        [HttpGet]
        public IActionResult Form(){
            return View();
        }
        
        [HttpGet]
        public IActionResult Game(Word selectedWord){
            Word gameWord = new Word();
            if(selectedWord.Value is null){
                var wordIndex = _library.RandomNumber(1);
                List<Word> wordList = _library.GetAllWords();
                gameWord.InitialWord = selectedWord.Value;
                gameWord = _library.HideLetters(wordIndex, wordList);
                gameWord.NoOfTries =  gameWord.Value.Count();
            } 
            else{
                gameWord.Category = selectedWord.Category;
                gameWord.InitialWord = selectedWord.InitialWord;
                gameWord.Value = selectedWord.Value;
                gameWord.NoOfTries = selectedWord.NoOfTries;
            }     
            return View(gameWord);            
        }

        [HttpPost]
        public IActionResult Enter(Word selectedWord){
            int NoOfTries = selectedWord.NoOfTries;
            char letter = selectedWord.Letter;
            Word newWord = new Word();
            newWord.Category = selectedWord.Category;
            newWord.InitialWord = selectedWord.InitialWord;
            selectedWord.NoOfTries = selectedWord.NoOfTries-1; 
            
            if(letter =='\0')
            {  newWord=selectedWord;
               newWord.Message ="You typed no letter!";
               newWord.NoOfTries=NoOfTries;
               newWord.Value = selectedWord.Value;
            }
            else
            {
                Boolean letterFound = _library.FindLetterInWord(letter, selectedWord);
                
                 
                if(selectedWord.NoOfTries<=0 && !letterFound)
                {
                    newWord=selectedWord;
                    newWord.Message ="Sorry, maybe next time. Press Game to play again";
                    newWord.NoOfTries=0;
                }
                else 
                {
                    if (!letterFound)
                    {
                        newWord=selectedWord;
                        newWord.Message ="Missed....";
                    }
                    else 
                    {
                        newWord = _library.exposeLetters(letter, selectedWord, selectedWord.Value);
                        if(newWord.Value==newWord.InitialWord)
                        {
                        newWord.Message = "WOW....You have found a word. CONGRATS! Press WORDGAME to play again.";
                        newWord.NoOfTries = 0;  
                        }
                        else 
                        {
                        newWord.Message = "Congratulations, you have found a letter!.";
                        }
                                    
                    }
                }
            }
      
        return View("Game", newWord);

        }


        [HttpPost]
        public IActionResult Create(Word newWord){
            _library.AddNewWord(newWord);
            return RedirectToAction("Index");
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }
    }
}



//   if (letterFound){
//                newWord = _library.exposeLetters(letter, selectedWord, selectedWord.Value);
//                newWord.Message = "Congratulations, you have found a letter!.";
//                newWord.NoOfTries = NoOfTries-1;
//             }
//             else {
//                 if(selectedWord.NoOfTries > 0 )
//                 {
//                     newWord=selectedWord;
//                     newWord.Message ="Missed....";
//                     newWord.NoOfTries = NoOfTries-1;
//                 }
//                else if(selectedWord.NoOfTries == 0 )
//                 {
//                     newWord=selectedWord;
//                     newWord.Message ="Sorry, maybe next time. Press Game to play again";
//                 }
                
//             }
//         return View("Game", newWord);

//         }
