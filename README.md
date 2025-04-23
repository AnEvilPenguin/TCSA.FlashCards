# FlashCards

A console base C# application used to help with learning topics that require recollection, like languages. The user
will set up a set of virtual flash cards with a question on one side and an answer on the other.
Implemented using SQL Server and the Data Transfer Object pattern.

# How to use

TODO

# Requirements

- [X] Allow users to create stacks of flashcards
- [X] Two tables, one for stacks and another for flashcards
  - [X] Tables are linked through foreign keys
- [X] Every flashcard MUST be a part of a stack
- [X] If a stack is deleted, the same should happen with the linked flashcards
- [X] Use DTOs to show the flashcards to the user without the id of the stack it belongs to
- [X] When showing a stack to the user, flashcard Ids should always start at 1 without any gaps between them
  - [X] If you have 10 cards and number 5 is deleted show Ids from 1 to 9.
- [X] After flash card functionality, create a "Study Session" area where the users will study the stacks
  - [X] All study sessions should be stored with date and score.
  - [X] The study and stack tables should be linked
    - [X] If a stack is deleted all its study sessions should also be deleted
- [X] The project should contain a call to the study table so that users can see all their study sessions
  - [X] Users should be able to view, but not update, or delete any study session   

## Stretch Goals

- [X] Report system
  - [X] Number of sessions per month per stack
  - [X] Average score per month per stack
- [ ] Sample data generation

# Features

# Challenges

# Lessons Learned

# Areas to Improve

# Resources Used

