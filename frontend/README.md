# In-Memory Database System - Angular Frontend

## Setup Instructions

### Prerequisites
- Node.js (v18 or higher)
- npm (comes with Node.js)

### Installation

1. Navigate to the frontend folder:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Start the development server:
```bash
npm start
```

4. Open your browser and go to:
```
http://localhost:4200
```

## Features

### 1. Create Table (Builder Pattern)
- Enter table name
- Add columns with name and type (String, Integer, Boolean)
- Create table with multiple columns

### 2. Insert Row (Command Pattern)
- Select a table
- Fill in values for each column
- Insert row into table

### 3. Query Data (Interpreter Pattern)
- Select table
- Choose column and operator (Equal, NotEqual, GreaterThan, LessThan)
- Enter value to compare
- View filtered results

### 4. View Tables (Facade Pattern)
- See all created tables
- View all rows in each table
- Delete rows

### 5. Clone Table (Prototype Pattern)
- Select a table to clone
- Creates independent copy with all data

## Technologies
- Angular 17 (Standalone Components)
- TypeScript
- CSS3
- Reactive Forms

## Note
This is a frontend-only implementation that simulates database operations in memory.
For a full implementation, you would need to create an ASP.NET Core Web API backend.
