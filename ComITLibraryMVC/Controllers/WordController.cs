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
    // public class Word{
    //     public string Value {get; set;}
    //     public string Category{get; set;}
    // }
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
                var wordIndex = _library.RandomNumber(1, 12);
                List<Word> wordList = _library.GetAllWords();
                gameWord.InitialWord = selectedWord.Value;
                gameWord = _library.HideLetters(wordIndex, wordList);
            } 
            else{
                gameWord.Category = selectedWord.Category;
                gameWord.InitialWord = selectedWord.InitialWord;
                gameWord.Value = selectedWord.Value;
            }     
            return View(gameWord);            
        }


        [HttpPost]
        public IActionResult Enter(Word selectedWord){
            char letter = selectedWord.Letter;
            Boolean letterFound = _library.FindLetterInWord(letter, selectedWord);
            Word newWord = new Word();
            newWord.Category = selectedWord.Category;
            newWord.InitialWord = selectedWord.InitialWord;
            if (letterFound){
               newWord = _library.exposeLetters(letter, selectedWord, selectedWord.Value);
               newWord.Message = "Congratulations, you have found a letter!.";
            }
            else {
                newWord=selectedWord;
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
