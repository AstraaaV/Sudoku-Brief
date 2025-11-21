#pragma once
#include <string>

// This is the 9x9 Sudoku board
class Grid
{
public:
	Grid(); // Sets everything to 0 aka empty

	// Loads a puzzle from a text file ("." = empty)
	bool loadFromFile(const std::string& filepath);
	void print() const; // Prints to console

	// Helpers to change / grab a number
	int get(int row, int col) const;
	int set(int row, int col, int value);

private:
	// This is the 9x9 board stored in a simple array
	int cells[9][9];
};