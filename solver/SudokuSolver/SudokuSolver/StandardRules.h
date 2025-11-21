#pragma once
#include "VariantRules.h"
#include "Rules.h"

class StandardRules : public VariantRules
{
public:
	bool isValid(const Grid& grid, int row, int col, int value) const override
	{
		return Rules::isValid(grid, row, col, value);
	}
};