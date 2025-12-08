# Virtual Trading Simulator

## 1. Project Overview
### Description
The Virtual Trading Simulator is a real-time security trading application that simulates market behavior and allows users 
to practice trading strategies without financial risk. The system supports two user roles: Admins who manage the market 
(create tickers, adjust volatility, manage users), and Traders who execute buy/sell orders and track portfolio performance.

This program was made with various Object-Oriented principles and design patterns in mind as to fit the criteria of the
ITSC-3122 (Design and Implementation of Object-Oriented Systems) Final Project and to challenge our knowledge of how an 
extendable codebase must be designed.

### Core Features
- **Real Time Price Updates** using random number generators with a number of factors contributing to each update.
- **Multiple Order Types and Strategies** with the allowance of both buy and sell orders using a trade strategy (like 
market or limit orders). Sell orders also have an accounting strategy (FIFO or LIFO) to select the securities to sell as
to better control profits/losses as is done in the real world.
- **Portfolio Tracking** with each holdings information and their correlated gain/loss.
- **Persistent Storage** using text files and file handlers.
- **Admin Controls** to allow for manipulation of the trading environment by editing and adding tickers, adding traders,
manipulating tickers volatility parameters, and changing the environments tick speed.

## 2. Quick Start Guide:

### Prerequisites:
- IDE (Preferably **Rider** as that is where it was tested)
- .Net 10.0 or higher

### Installation
- Clone the Project using the following git command:
```
git clone https://github.com/ralvar10/Virtual_Trading_Simulator_Project.git
```
- Click on the project folder and click run.

### Included Files
Test Data is provided within the repository and located within the /docs directory, it is suggested for you to keep these
files unchanged, however, you can modify them to fit your liking.

## 3. Required OOP Features (with File & Line References)

### Inheritance

| OOP Feature | File Name                                                                       | Line Numbers                                        | Reasoning / Purpose                                                                                                                                                                                                                                                           |
|-------------|---------------------------------------------------------------------------------|-----------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Inheritance Example 1 | User.cs (base)<br>`Admin.cs` (derived)<br>`Trader.cs` (derived)                 | User.cs: 3-40<br>Admin.cs: 9 <br>Trader.cs: 9       | Admin inherits from User base class to share common behavior (Login(), LogOut()) and the Username property. This eliminates code duplication and ensures consistent user management across both admin and trader roles.                                                       |
| Inheritance Example 3 | Order.cs (abstract base)<br>`BuyOrder.cs` (derived)<br>`SellOrder.cs` (derived) | Order.cs: 7-56<br>BuyOrder.cs: 7<br>SellOrder.cs: 9 | Both BuyOrder and SellOrder inherit from abstract Order base class, sharing common properties (Quantity, Value, Status, Security) and the order placement structure. Each derived class implements specific validation and execution logic through abstract method overrides. |

### Interface Implementation

| OOP Feature | File Name | Line Numbers | Reasoning / Purpose                                                                                                                                                                                                                                                                      |
|-------------|-----------|--------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Interface Implementation 1 | `IFileHandler.cs` (interface)<br>`TickerFileHandler.cs` (impl)<br>`UserFileHandler.cs` (impl) | IFileHandler.cs: 3-9<br>TickerFileHandler: 7-163<br>UserFileHandler: 8-198 | IFileHandler defines a method outline for file operations (LoadFromFile, WriteToFile) that both ticker and user file handlers must implement. This allows the system to treat different file types uniformly while maintaining similar logic.                                            |
| Interface Implementation 2 | `ITradeStrategy.cs` (interface)<br>`MarketStrategy.cs` (impl)<br>`LimitStrategy.cs` (impl) | ITradeStrategy.cs: 3-7<br>MarketStrategy: 5-18<br>LimitStrategy: 5-35 | ITradeStrategy enables interchangeable order execution algorithms. Market orders execute immediately, while limit orders wait for price conditions. This implements the Strategy pattern for runtime algorithm selection.                                                                |
| Interface Implementation 3 | `IAccountingStrategy.cs` (interface)<br>`FifoStrategy.cs` (impl)<br>`LifoStrategy.cs` (impl) | IAccountingStrategy.cs: 5-9<br>FifoStrategy: 5-47<br>LifoStrategy: 5-47 | IAccountingStrategy provides different methods for selecting which holdings to sell. FIFO sells oldest shares first (by purchase time), while LIFO sells newest shares. This affects profit calculations and allows some consideration of the tax implications in real trading scenarios. |
| Interface Implementation 4 | `IStatistics.cs` (interface)<br>`TraderStatistics.cs` (impl)<br>`TickerStatistics.cs` (impl)<br>`HoldingStatistics.cs` (impl)<br>`OrderStatistics.cs` (impl) | IStatistics.cs: 3-7<br>TraderStatistics: 7-57<br>TickerStatistics: 7-96<br>HoldingStatistics: 7-80<br>OrderStatistics: 7-31 | IStatistics defines an interface for statistics display and updates across different entities (traders, tickers, holdings, orders). Each implementation calculates and displays relevant metrics, enabling consistent statistics handling throughout the application.                    |
| Interface Implementation 5 | `ITickHandler.cs` (interface)<br>`StockTickHandler.cs` (impl) | ITickHandler.cs: 3-11<br>StockTickHandler: 7-101 | ITickHandler abstracts the price update mechanism, allowing the system to start, pause, stop, and configure tick rates. The interface enables future implementations (with the potential for real-time market data feeds) without changing dependent code.                               |
| Interface Implementation 6 | `ITickerRepository.cs` (interface)<br>`StockMarket.cs` (impl) | ITickerRepository.cs: 3-10<br>StockMarket: 5-50 | ITickerRepository abstracts ticker storage and retrieval.|

### Polymorphism

| OOP Feature | File Name                                                                                                                                          | Line Numbers                                                                                            | Reasoning / Purpose                                                                                                                                                                                                                                                                                 |
|-------------|----------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Polymorphism Example 1 | `Order.cs` (abstract methods)<br>`BuyOrder.cs` (override)<br>`SellOrder.cs` (override) <br/>`Trader.cs` (usage)                                    | Order.cs: 42-54<br>BuyOrder: 17-26, 28-81<br>SellOrder: 23-115<br/>Trader: 31                           | The abstract methods Validate() and PlaceOrder() are overridden with different implementations. BuyOrder validates sufficient balance, while SellOrder validates sufficient holdings. PlaceOrder implementation differs as BuyOrder adds holdings while SellOrder removes them and calculates gains. |
| Polymorphism Example 2 | `ITradeStrategy.cs` (interface)<br>`MarketStrategy.cs` (impl)<br>`LimitStrategy.cs` (impl)<br/> `BuyOrder.cs` (usage) <br/> `SellOrder.cs` (usage) | ITradeStrategy: 7<br>MarketStrategy: 13-17<br>LimitStrategy: 21-34<br/>BuyOrder: 31 <br/> SellOrder: 41 | The ShouldExecute() method has different behavior based on the strategy. Market strategy always returns true for immediate execution, while limit strategy compares current price to target price and returns true/false conditionally based on order type (buy below, sell above).                 |
| Polymorphism Example 3 | `IAccountingStrategy.cs` (interface) <br> `FifoStrategy.cs` (impl)<br>`LifoStrategy.cs` (impl)<br/> `SellOrder.cs` (usage)                         | IAccountingStrategy: 8<br>FifoStrategy: 13-45<br>LifoStrategy: 14-46 <br/> SellOrder: 27, 68            | The SelectHoldings() method exhibits polymorphic behavior. FIFO sorts holdings by purchase time (ascending), while LIFO sorts descending. Each has the same method signature with different behavior based on the concrete strategy instance.                                                       |
| Polymorphism Example 4 | `User.cs` (base class)<br>`Admin.cs` and `Trader.cs` (derived)<br/> `MenuManager.cs` (usage)                                                       | User.cs: 17-29<br>Entire Admin and Trader classes<br/> MenuManager: 113-124                             | Admin and Trader objects can be stored in a List<User> and treated differently. The Login() method works identically for both types, and type checking (is Admin, is Trader) enables role-specific menu display without modifying the User base class.                                       |

### Access Modifiers

| Element | File Name | Line Numbers | Access Modifier | Reasoning                                                                                                                                                                                                   |
|---------|-----------|--------------|-----------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| User._password | `User.cs` | 5            | `private readonly` | Password should never be directly accessible or modifiable. Only accessible through Login() validation method and GetPassword() (used for writing to files).                                                |
| Trader._balance | `Trader.cs` | 11           | `private` | Balance can only be modified through UpdateBalance() method which includes validation (prevents negative balances). Direct access would bypass important business rules. Retrieved via GetBalance() getter. |
| Order.TradeStrategy | `Order.cs` | 18           | `protected` | Trade strategy is accessible to derived classes (BuyOrder, SellOrder) for execution logic but hidden from external code.                                                                                    |
| Ticker._price | `Ticker.cs` | 9            | `private` | Price is accessed only through GetPrice() and modified through SetPrice() and UpdatePrice(), both of which include thread-safety locks.                                                                     |
| Ticker._priceLock | `Ticker.cs` | 13           | `private readonly` | Lock object for thread safety should never be accessible outside the class.                          |
| Admin.AddTicker() | `Admin.cs` | 20-80        | `public` | Admin functionality must be accessible to the menu system. Public methods represent the admin's capabilities and are called by AdminMenu class.                                                             |
| StockTickHandler.Tick() | `StockTickHandler.cs` | 47-58        | `private` | Tick method is an internal implementation detail called by the background task. Making it private prevents external code from triggering unexpected price updates or interfering with the tick cycle.       |

### Struct

| OOP Feature | File Name | Line Numbers | Reasoning / Purpose                                                                                                                                                      |
|-------------|-----------|--------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Struct Example | `VolatilityParameters.cs` | 3-24 | Defines storage for volatility configurations. Uses four doubles (CurrentVolatility, Sentiment, MinVolatility, and MaxVolatility) to aid in price updates within Ticker. |

### Enum

| OOP Feature | File Name | Line Numbers | Reasoning / Purpose                                                                                                                                                                                                                                                              |
|-------------|-----------|--------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Enum Example | `OrderStatus.cs` | 3-8 | Defines the state of an order: Pending (awaiting execution), Filled (successfully executed), Canceled (manually canceled by user), and Failed (validation or execution error). The order status determines how an order displays and whether an order can be modified. |

### Data Structures

| OOP Feature | File Name | Line Numbers | Reasoning / Purpose                                                                                                                                                                                                                                                                         |
|-------------|-----------|--------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Dictionary | `HoldingManager.cs` | 8            | Uses Dictionary<string, List<Holding>> to store holdings indexed by ticker symbol. Provides O(1) lookup when searching for holdings by symbol, which is the most common operation. Each ticker symbol maps to a list of holdings. |
| List (Generic) | `StockMarket.cs` | 5            | Uses List<Ticker> to store all available tickers in the market. Provides dynamic array functionality with O(1) append and O(n) search operations. Search is acceptable since ticker count is relatively small in this application.                                                          |
| List (Generic) | `OrderHistory.cs` | 7            | Uses List<Order> to maintain chronological order history for each trader. Enables sequential access for displaying order history and filtering by status (pending, filled, canceled).                                                 |
| IReadOnlyList | `OrderHistory.cs` | 20, 28       | Returns IReadOnlyList<Order> to prevent external modification of order history while allowing iteration over its data.                                                                        |

### Console I/O

| OOP Feature | File Name | Line Numbers | Reasoning / Purpose                                                                                                                                                                                                                                                     |
|-------------|-----------|--------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Console Input | `MenuManager.cs` | 113-152      | Implements user authentication through console input. Prompts for username and password, validates input isn't empty, searches user list, and calls Login() method.                                                                                                     |
| Console Output | `TraderStatistics.cs` | 46-64        | Displays trader statistics using Console.WriteLine() with formatted output. Includes conditional color formatting (Green for gains, Red for losses) using Console.ForegroundColor.                                                                                      |
| Console Input & Output (Menu) | `TraderMenu.cs` | 25-66        | Shows complete menu system with input prompting and option selection. Displays trader balance, menu options, reads user choice, and executes corresponding action using switch statement. D                                                                             |
| File I/O (Read) | `TickerFileHandler.cs` | 31-91        | Reads ticker data from text file using File.ReadAllLines(). Parses each line, creates Ticker objects, and adds to repository.                                                                                                                                           |
| File I/O (Write) | `UserFileHandler.cs` | 124-173      | Writes user and holdings data to text file using File.WriteAllText(). Formats data with section markers [USER] and [HOLDINGS].                                                                                                                                          |
| Complex I/O (Order Placement) | `TraderMenu.cs` | 120-258      | Order placement process with multiple input prompts (symbol, type, quantity, strategy, limit price, accounting method) and confirmation messages. Shows validation at each step, temporary pause of background tick handler during input, and an order summary display. |

---

## 4. Design Patterns (with File & Line References)

### Pattern 1: Strategy Pattern (Behavioral)

| Pattern Name | Category | File Name | Line Numbers                                                                                               | Rationale                                                                                                                                                                                                                                                                                                                                                                                                                                           |
|--------------|----------|-----------|------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Strategy | Behavioral | `ITradeStrategy.cs`<br>`MarketStrategy.cs`<br>`LimitStrategy.cs`<br>`Order.cs` (usage)<br>`OrderFactory.cs` (creation) | ITradeStrategy: 3-7<br>MarketStrategy: 5-17<br>LimitStrategy: 5-35<br>Order: 18, 33<br>OrderFactory: 28-38 | The Strategy pattern enables selection of order execution algorithms without modifying the Order class. Market orders execute immediately while limit orders wait for target prices. This solves the problem of having multiple execution strategies without creating separate order types. The pattern allows new trading strategies to be added without changing existing code, adhering to the Open/Closed Principle. |

**Implementation Details:**
- **Context**: `Order` class holds a reference to `ITradeStrategy`
- **Strategy Interface**: `ITradeStrategy` with ShouldExecute() method
- **Concrete Strategies**: `MarketStrategy` (always executes), `LimitStrategy` (conditional execution)
- **Usage**: `OrderFactory` selects strategy based on user input. Order.PlaceOrder() delegates execution decision to strategy

### Pattern 2: Singleton Pattern (Creational)

| Pattern Name | Category | File Name | Line Numbers                                                       | Rationale                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
|--------------|----------|-----------|--------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Singleton | Creational | `StockMarket.cs`<br>`StockTickHandler.cs`<br>`OrderFactory.cs` | StockMarket: 8-20<br>StockTickHandler: 7-27<br>OrderFactory: 10-20 | The Singleton pattern ensures only one instance of the repository is in use. Must be a singleton because all tickers must be stored in a single repository to ensure data consistency. StockTickHandler must be singular to prevent multiple conflicting price update loops. OrderFactory is a singleton to centralize order creation. |

**Implementation Details:**
- **Private constructor**: Prevents external instantiation
- **Static instance field**: Holds the single instance 
- **Public static accessor**: GetRepository(), GetInstance(),`GetFactory() methods

### Pattern 3: Factory Method Pattern (Creational)

| Pattern Name | Category | File Name | Line Numbers | Rationale |
|--------------|----------|-----------|--------------|-----------|
| Factory Method | Creational | `OrderFactory.cs` | 22-66        | The Factory pattern encapsulates the logic of creating orders with different strategy combinations. Order creation requires selecting trade strategies (Market/Limit), accounting strategies (FIFO/LIFO), and order types (Buy/Sell) based on string parameters from user input.  |

**Implementation Details:**
- **Factory Method**: CreateOrder() with parameters for trader, quantity, security, order type, and strategy names
- **Return**: Returns `Order` base class (can be `BuyOrder or `SellOrder`)
- **Strategy Selection**: Factory contains switch statements to instantiate correct `ITradeStrategy` and `IAccountingStrategy` based on string parameters
- **Encapsulation**: Hides conditional logic and default value handling 

### Pattern 4: Template Method Pattern (Behavioral)

| Pattern Name | Category | File Name | Line Numbers                                                                                                | Rationale                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
|--------------|----------|-----------|-------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Template Method | Behavioral | `Order.cs` (abstract template)<br>`BuyOrder.cs` (concrete)<br>`SellOrder.cs` (concrete) | Order: 42-54 (abstract methods)<br>BuyOrder: 29-77 (PlaceOrder impl)<br>SellOrder: 39-109 (PlaceOrder impl) | The Template Method pattern defines the skeleton of the order execution algorithm in the abstract Order class while letting subclasses override specific steps. The template structure includes: wait for strategy approval, validate, execute, and update status. Both BuyOrder and SellOrder follow this structure but implement Validate() and core execution logic differently. |

**Implementation Details:**
- **Abstract Class**: Order defines template structure
- **Template Method**: PlaceOrder() in Order calls abstract Validate() method

## 5. Design Decisions
**Key Abstractions:**
- **Strategy Pattern for Orders**: Trade strategies (Market/Limit) and accounting strategies (FIFO/LIFO) are interchangeable through interfaces, allowing new algorithms without modifying order execution code.
- **Repository Pattern for Data**: `ITickerRepository` abstracts storage.
- **Order Hierarchy with Template Method**: Abstract `Order` class defines execution. `BuyOrder` and `SellOrder` override specific steps (validation, execution) while sharing a common process for execution.

**Critical Tradeoffs:**
- **Threading complexity vs. Real-time simulation**: Background price updates require thread synchronization (locks on ticker prices) but provide realistic market behavior.
- **Plain text passwords vs. Security**: Bad for security but fine for an academic/presentational setting

