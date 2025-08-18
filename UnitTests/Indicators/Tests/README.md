# Technical Analysis Indicators - Test Suite Documentation

## Overview

This directory contains a comprehensive test suite for the TechAnalysis library indicators. The test suite has been significantly expanded to provide thorough coverage of all indicator functionality, edge cases, and integration scenarios.

## Test Structure

### Test Organization

The tests are organized into separate files by indicator category:

- **`MovingAverageIndicatorTests.cs`** - Tests for SMA, EMA, DEMA, TEMA, WMA, SMMA, and KAMA indicators
- **`OscillatorIndicatorTests.cs`** - Tests for CCI and Stochastics indicators  
- **`ChannelIndicatorTests.cs`** - Tests for Bollinger Bands and Keltner Channel indicators
- **`StatisticsIndicatorTests.cs`** - Tests for ATR, MACD, and Standard Deviation indicators
- **`RegressionIndicatorTests.cs`** - Tests for Linear Regression indicator
- **`EdgeCaseAndIntegrationTests.cs`** - Edge cases, integration tests, and performance tests
- **`LegacyIndicatorBaselineTests.cs`** - Legacy baseline tests (renamed from IndicatorTests.cs)

### Test Categories

#### 1. Baseline Accuracy Tests
- Verify indicators calculate expected values against known baselines
- Test mathematical correctness of indicator formulas
- Ensure consistency with reference implementations

#### 2. Parameter Validation Tests
- Test constructor parameter validation
- Verify appropriate exceptions are thrown for invalid inputs
- Test boundary conditions for parameter ranges

#### 3. State Management Tests
- Test `IsPrimed` property behavior
- Verify `Count` property accuracy
- Test indexer access functionality
- Validate result ordering and storage

#### 4. Price Type Tests
- Test indicators with different `PriceToUse` values (Open, High, Low, Close)
- Ensure consistent behavior across all price types
- Verify price extraction logic

#### 5. Edge Case Tests
- Test with extreme values (very large/small numbers)
- Test with identical prices (zero volatility)
- Test with flat price ranges (High = Low)
- Test numerical stability and overflow protection

#### 6. Integration Tests
- Test indicator combinations and chaining
- Test factory method functionality (`MaFactory`)
- Test real-world usage scenarios
- Verify indicator interoperability

#### 7. Performance Tests
- Test with large datasets (10,000+ candles)
- Verify reasonable execution times
- Test memory usage patterns

## Test Naming Conventions

All tests follow descriptive naming patterns:

```
[IndicatorName]_[Scenario]_[ExpectedBehavior]
```

Examples:
- `SmaIndicator_WithValidParameters_ShouldCalculateCorrectValues`
- `BollingerBandsIndicator_WithZeroStandardDeviation_ShouldHandleCorrectly`
- `StochasticsIndicator_WithInvalidParameters_ShouldThrowArgumentOutOfRangeException`

## Key Testing Principles

### 1. Comprehensive Coverage
- Every public method and property is tested
- All constructor overloads are validated
- Edge cases and error conditions are covered

### 2. Descriptive Assertions
- Uses FluentAssertions for readable test failures
- Provides detailed error messages with expected vs actual values
- Includes context about what is being tested

### 3. Isolated Tests
- Each test is independent and can run in isolation
- Tests use deterministic data (fixed random seeds)
- No shared state between tests

### 4. Realistic Test Data
- Uses realistic price data patterns
- Tests with various volatility levels
- Includes trending and sideways market conditions

## Test Data Patterns

### Standard Test Candles
- Generated with realistic OHLC relationships
- Includes proper price progression
- Uses fixed random seed for reproducibility

### Specialized Test Data
- **Low Volatility**: Minimal price movement for stability testing
- **High Volatility**: Large price swings for stress testing
- **Constant Prices**: Identical OHLC values for edge case testing
- **Linear Trends**: Perfect upward/downward trends for regression testing

## Validation Methods

### Numerical Precision
- Uses appropriate tolerance levels for floating-point comparisons
- Accounts for cumulative rounding errors in iterative calculations
- Validates precision requirements for financial calculations

### Mathematical Correctness
- Verifies indicator formulas against mathematical definitions
- Tests known mathematical properties (e.g., SMA linearity)
- Validates statistical properties where applicable

## Running Tests

### All Tests
```bash
dotnet test
```

### Specific Test Class
```bash
dotnet test --filter "ClassName=MovingAverageIndicatorTests"
```

### Specific Test Method
```bash
dotnet test --filter "MethodName=SmaIndicator_WithValidParameters_ShouldCalculateCorrectValues"
```

### Exclude Performance Tests
```bash
dotnet test --filter "TestCategory!=Performance"
```

## Test Metrics

The expanded test suite provides:

- 160+ test methods (up from 16 original tests)
- Code coverage collected via Coverlet (Cobertura); see CI artifacts for reports
- Comprehensive parameter validation for all constructors
- **Edge case coverage** for numerical stability
- **Integration testing** for real-world scenarios
- **Performance benchmarking** for large datasets

## Maintenance Guidelines

### Adding New Tests
1. Follow the established naming conventions
2. Use appropriate test categories and organization
3. Include both positive and negative test cases
4. Add comprehensive parameter validation tests
5. Document any special test data requirements

### Updating Existing Tests
1. Maintain backward compatibility with baseline tests
2. Update test names to reflect changes in behavior
3. Ensure test isolation is preserved
4. Update documentation for any new test patterns

### Test Data Management
1. Use deterministic test data generation
2. Document any external test data dependencies
3. Keep test data realistic and representative
4. Maintain separate data sets for different test scenarios

## Known Limitations

1. **Baseline Data**: Some baseline tests depend on pre-calculated expected values
2. **Floating Point Precision**: Tests account for inherent floating-point precision limitations
3. **Performance Tests**: Performance benchmarks may vary by hardware
4. **External Dependencies**: Tests assume stable behavior of underlying .NET math functions

## Future Enhancements

Potential areas for further test expansion:

1. **Stress Testing**: Even larger datasets and extreme scenarios
2. **Concurrency Testing**: Multi-threaded indicator usage
3. **Memory Profiling**: Detailed memory usage analysis
4. **Benchmark Comparisons**: Performance comparisons with other libraries
5. **Property-Based Testing**: Automated test case generation
6. **Historical Data Testing**: Tests with real market data
