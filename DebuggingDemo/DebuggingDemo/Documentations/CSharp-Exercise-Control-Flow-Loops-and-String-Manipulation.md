# C# Exercise – Control Flow Through Loops and String Manipulation

> **Note:** The result of this exercise must be shown to and approved by the teacher before the exercise can be considered complete.

The entire exercise may be written in the `Program` class, with the menu inside the `Main` method.

## Main Menu

Create a main menu that keeps the program running and informs the user how to use it.

To create the main menu, do the following:

1. Tell the user that they are in the main menu and that they can navigate by entering numbers to test the different features.
2. Create the basic structure of a `switch` statement. Initially, it should have two cases:
   - `0` closes the program.
   - `default` tells the user that their input is invalid.
3. Create an indefinite iteration that does not end until the program is told to stop. Do this by creating a `bool` variable and using it as the condition of a `while` loop.
4. Expand the menu with options that run the remaining exercises.

## Menu Option 1: Youth or Senior Citizen

To demonstrate `if` statements, implement a program for a fictional local cinema that checks whether a person qualifies for the youth price or the senior citizen price based on their age.

Add `case 1` to the main menu for this feature. The menu text must also explain this option.

Use a nested `if` statement and follow this process:

1. The user enters their age as a number.
2. The program converts the value from a string to an `int`.
3. The program checks whether the person is a youth (under 20 years old).
4. If true, the program prints: `Youth price: SEK 80`.
5. Otherwise, the program checks whether the person is a senior citizen (over 64 years old).
6. If true, the program prints: `Senior citizen price: SEK 90`.
7. Otherwise, the program prints: `Standard price: SEK 120`.

### Calculate the Price for a Group

The program must also be able to calculate the price for an entire group. Add this feature to the main menu as `case 2`. It is also acceptable to place it in a submenu.

First, ask how many people are going to the cinema. Then ask for the age of each person. Finally, print a summary containing:

- The number of people
- The total cost for the entire group

## Menu Option 2: Repeat Ten Times

To practice another kind of iteration, implement a `for` loop that repeats text entered by the user ten times. Do not use ten separate `Console.Write(input)` statements; use a loop to perform the repetition.

Add `case 3` to the main menu for this feature. The menu text must also explain this option.

Process:

1. The user enters any text.
2. The program stores the text in a variable.
3. Using a `for` loop, the program prints the text ten times on the same line, without line breaks.

Example output:

```text
1. Input, 2. Input, 3. Input, etc.
```

## Menu Option 3: The Third Word

You have previously learned how to convert strings to integers, for example with `int.Parse` and `int.TryParse`. In this exercise, you will split a string into separate parts.

The user enters a sentence, and the program splits it into words with the string `.Split(char)` method. Split the string at each space. Store the result in a variable because the method returns multiple strings.

Add `case 4` to the main menu for this feature. The menu text must also explain this option.

Process:

1. The user enters a sentence containing at least three words.
2. The program splits the string at each space using the `Split` method.
3. The program selects the third string—the third word—from the input.
4. The program prints the third word.

## Documentation

Remember to comment your code carefully so that you and others can understand it easily in the future.

## Extra Tasks for Those Who Have Time

1. Validate all user input and ensure that the program does not crash when the input is invalid.
2. Children under five years old and senior citizens over 100 years old enter for free.
3. Handle multiple consecutive spaces in the third-word exercise.
4. Add anything else that seems interesting or that you would like to practice.

Good luck!
