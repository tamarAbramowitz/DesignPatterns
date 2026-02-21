import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Column {
  name: string;
  type: string;
}

interface Table {
  name: string;
  columns: Column[];
  rows: any[];
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
<div class="container">
  <div class="header">
    <h1>🗄️ In-Memory Database System</h1>
    <p>Design Patterns Project - Angular Frontend</p>
  </div>

  <div class="tabs">
    <button class="tab" [class.active]="activeTab === 'create'" (click)="setTab('create')">
      ➕ Create Table
    </button>
    <button class="tab" [class.active]="activeTab === 'insert'" (click)="setTab('insert')">
      📝 Insert Row
    </button>
    <button class="tab" [class.active]="activeTab === 'query'" (click)="setTab('query')">
      🔍 Query
    </button>
    <button class="tab" [class.active]="activeTab === 'view'" (click)="setTab('view')">
      📊 View Tables
    </button>
    <button class="tab" [class.active]="activeTab === 'clone'" (click)="setTab('clone')">
      📋 Clone Table
    </button>
  </div>

  <div class="content">
    <div *ngIf="message" [class]="'alert alert-' + messageType">
      {{ message }}
    </div>

    <div *ngIf="activeTab === 'create'">
      <h2>Create New Table</h2>
      
      <div class="form-group">
        <label>Table Name:</label>
        <input type="text" [(ngModel)]="newTableName" placeholder="Enter table name">
      </div>

      <div class="column-builder">
        <h3>Add Columns</h3>
        <div class="column-item">
          <input type="text" [(ngModel)]="columnName" placeholder="Column name">
          <select [(ngModel)]="columnType">
            <option value="String">String</option>
            <option value="Integer">Integer</option>
            <option value="Boolean">Boolean</option>
          </select>
          <button class="btn btn-success" (click)="addColumn()">Add</button>
        </div>

        <div *ngFor="let col of newColumns; let i = index" class="column-item">
          <span>{{ col.name }} ({{ col.type }})</span>
          <button class="btn btn-danger" (click)="removeColumn(i)">Remove</button>
        </div>
      </div>

      <button class="btn btn-primary" (click)="createTable()">Create Table</button>
    </div>

    <div *ngIf="activeTab === 'insert'">
      <h2>Insert Row</h2>
      
      <div class="form-group">
        <label>Select Table:</label>
        <select [(ngModel)]="selectedTable">
          <option value="">-- Select Table --</option>
          <option *ngFor="let table of tables" [value]="table.name">{{ table.name }}</option>
        </select>
      </div>

      <div *ngIf="selectedTable">
        <div *ngFor="let col of getTableColumns(selectedTable)" class="form-group">
          <label>{{ col.name }} ({{ col.type }}):</label>
          <input type="text" [(ngModel)]="rowData[col.name]" [placeholder]="'Enter ' + col.name">
        </div>

        <button class="btn btn-primary" (click)="insertRow()">Insert Row</button>
      </div>
    </div>

    <div *ngIf="activeTab === 'query'">
      <h2>Query Data</h2>
      
      <div class="form-group">
        <label>Select Table:</label>
        <select [(ngModel)]="queryTable">
          <option value="">-- Select Table --</option>
          <option *ngFor="let table of tables" [value]="table.name">{{ table.name }}</option>
        </select>
      </div>

      <div *ngIf="queryTable">
        <div class="form-group">
          <label>Column (leave empty for all):</label>
          <select [(ngModel)]="queryColumn">
            <option value="">-- All Rows --</option>
            <option *ngFor="let col of getTableColumns(queryTable)" [value]="col.name">{{ col.name }}</option>
          </select>
        </div>

        <div *ngIf="queryColumn" class="form-group">
          <label>Operator:</label>
          <select [(ngModel)]="queryOperator">
            <option value="Equal">Equal (=)</option>
            <option value="NotEqual">Not Equal (!=)</option>
            <option value="GreaterThan">Greater Than (>)</option>
            <option value="LessThan">Less Than (<)</option>
          </select>
        </div>

        <div *ngIf="queryColumn" class="form-group">
          <label>Value:</label>
          <input type="text" [(ngModel)]="queryValue" placeholder="Enter value">
        </div>

        <button class="btn btn-primary" (click)="queryData()">Query</button>

        <div *ngIf="queryResults.length > 0" class="table-container">
          <h3>Results ({{ queryResults.length }} rows)</h3>
          <table>
            <thead>
              <tr>
                <th *ngFor="let col of getTableColumns(queryTable)">{{ col.name }}</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let row of queryResults">
                <td *ngFor="let col of getTableColumns(queryTable)">{{ row[col.name] }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <div *ngIf="activeTab === 'view'">
      <h2>All Tables</h2>
      
      <div *ngFor="let table of tables" style="margin-bottom: 30px;">
        <h3>{{ table.name }} ({{ table.rows.length }} rows)</h3>
        
        <div class="table-container">
          <table>
            <thead>
              <tr>
                <th *ngFor="let col of table.columns">{{ col.name }}</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let row of table.rows; let i = index">
                <td *ngFor="let col of table.columns">{{ row[col.name] }}</td>
                <td>
                  <button class="btn btn-danger" (click)="deleteRow(table.name, i)">Delete</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <div *ngIf="tables.length === 0">
        <p>No tables created yet. Go to "Create Table" to add one.</p>
      </div>
    </div>

    <div *ngIf="activeTab === 'clone'">
      <h2>Clone Table</h2>
      
      <div class="form-group">
        <label>Select Table to Clone:</label>
        <select [(ngModel)]="selectedTable">
          <option value="">-- Select Table --</option>
          <option *ngFor="let table of tables" [value]="table.name">{{ table.name }}</option>
        </select>
      </div>

      <button class="btn btn-primary" (click)="cloneTable()" [disabled]="!selectedTable">
        Clone Table
      </button>
    </div>
  </div>
</div>
  `,
  styles: []
})
export class AppComponent {
  activeTab = 'create';
  tables: Table[] = [];
  message = '';
  messageType = '';

  newTableName = '';
  newColumns: Column[] = [];
  columnName = '';
  columnType = 'String';

  selectedTable = '';
  rowData: any = {};

  queryTable = '';
  queryColumn = '';
  queryOperator = 'Equal';
  queryValue = '';
  queryResults: any[] = [];

  setTab(tab: string) {
    this.activeTab = tab;
    this.message = '';
  }

  addColumn() {
    if (this.columnName) {
      this.newColumns.push({ name: this.columnName, type: this.columnType });
      this.columnName = '';
      this.columnType = 'String';
    }
  }

  removeColumn(index: number) {
    this.newColumns.splice(index, 1);
  }

  createTable() {
    if (!this.newTableName || this.newColumns.length === 0) {
      this.showMessage('Please enter table name and add columns', 'error');
      return;
    }

    const table: Table = {
      name: this.newTableName,
      columns: [...this.newColumns],
      rows: []
    };

    this.tables.push(table);
    this.showMessage(`Table '${this.newTableName}' created successfully!`, 'success');
    
    this.newTableName = '';
    this.newColumns = [];
  }

  insertRow() {
    const table = this.tables.find(t => t.name === this.selectedTable);
    if (!table) {
      this.showMessage('Please select a table', 'error');
      return;
    }

    const row: any = {};
    table.columns.forEach(col => {
      row[col.name] = this.rowData[col.name] || '';
    });

    table.rows.push(row);
    this.showMessage('Row inserted successfully!', 'success');
    this.rowData = {};
  }

  queryData() {
    const table = this.tables.find(t => t.name === this.queryTable);
    if (!table) {
      this.showMessage('Please select a table', 'error');
      return;
    }

    if (!this.queryColumn) {
      this.queryResults = table.rows;
      this.showMessage(`Found ${table.rows.length} rows`, 'success');
      return;
    }

    this.queryResults = table.rows.filter(row => {
      const value = row[this.queryColumn];
      const compareValue = this.queryValue;

      switch (this.queryOperator) {
        case 'Equal':
          return value == compareValue;
        case 'NotEqual':
          return value != compareValue;
        case 'GreaterThan':
          return Number(value) > Number(compareValue);
        case 'LessThan':
          return Number(value) < Number(compareValue);
        default:
          return true;
      }
    });

    this.showMessage(`Found ${this.queryResults.length} rows`, 'success');
  }

  cloneTable() {
    const table = this.tables.find(t => t.name === this.selectedTable);
    if (!table) {
      this.showMessage('Please select a table', 'error');
      return;
    }

    const clonedTable: Table = {
      name: table.name + '_Clone',
      columns: [...table.columns],
      rows: table.rows.map(row => ({ ...row }))
    };

    this.tables.push(clonedTable);
    this.showMessage(`Table '${table.name}' cloned successfully!`, 'success');
  }

  deleteRow(tableName: string, index: number) {
    const table = this.tables.find(t => t.name === tableName);
    if (table) {
      table.rows.splice(index, 1);
      this.showMessage('Row deleted successfully!', 'success');
    }
  }

  showMessage(msg: string, type: string) {
    this.message = msg;
    this.messageType = type;
    setTimeout(() => {
      this.message = '';
    }, 3000);
  }

  getTableColumns(tableName: string): Column[] {
    const table = this.tables.find(t => t.name === tableName);
    return table ? table.columns : [];
  }
}
