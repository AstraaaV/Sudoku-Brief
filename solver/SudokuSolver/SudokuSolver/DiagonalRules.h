#pragma once
#include "VariantRules.h"
#include "Rules.h"

class DiagonalRules : public VariantRules
{
public:
	// Checks if putting a number here follows the diagonal rules
	bool isValid(const Grid& grid, int row, int col, int value) const override;
};