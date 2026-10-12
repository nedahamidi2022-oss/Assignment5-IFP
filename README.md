# Assignment 5: Composing Functions Using Pipelining
**Course:** Introduction to Functional Programming  
**Academic Year:** 2026–2027  
**Student Name:** Neda Farhat  
**Group:** IT-2511 (Astana IT University)

---

## 1. Project Overview and Topic
* **Topic:** Delivery Quote Calculation Workflow.
* **Intended Users:** E-commerce platforms and logistics managers needing automated, rule-based delivery cost estimations.
* **Input Fields:** 
  * `OrderId` (string): Unique identifier for each delivery request.
  * `DistanceKm` (double): Delivery distance in kilometers.
  * `IsExpress` (bool): Flag indicating whether the delivery is express.
* **Business Rules:** 
  * Invalid records (such as negative distances, missing IDs, or null requests) are filtered out at the application boundary during Stage 1.
  * Calculations are performed using pure transformation stages separated from input/output operations.
  * Two configurable pricing policies (Standard and Express) are injected via higher-order functions (`Func<double, double>`).

---

## 2. Workflow Architecture & Pipeline Stages
The application processes data through three distinct pure stages:
1. **Stage 1 (Validation & Normalization):** Filters raw inputs and rejects malformed or missing data, returning an immutable `IReadOnlyList<ValidatedRequest>`.
2. **Stage 2 (Cost Calculation):** Applies a higher-order pricing delegate to compute total delivery costs, producing an immutable `IReadOnlyList<PricedQuote>`.
3. **Stage 3 (Summary & Ordering):** Sorts and structures the final quotes by total cost.

---

## 3. Test Cases & Verification
The solution includes 6 cumulative automated tests implemented using **xUnit** covering:
1. **Normal Workflow:** Valid records successfully pass through the pipeline and match expected calculations.
2. **Empty Dataset:** Valid empty input datasets yield an empty result without crashing.
3. **Boundary Value:** Exact threshold values (e.g., zero distance) are correctly evaluated and included.
4. **Invalid and Missing Input:** Malformed fields, negative numbers, and null entries are successfully rejected.
5. **Two Policies:** Both standard and express policies are executed on identical inputs, yielding distinct, correct results.
6. **Purity and Stages:** Asserts that input collections and caller-owned data remain strictly unchanged.

---

## 4. Build, Run, and Test Instructions

Prerequisites:
* **.NET SDK:** Version 8.0 or higher.

### Build the Solution
Open a terminal in the root solution folder (`Assignment 5 IFP`) and run:
```bash
dotnet build