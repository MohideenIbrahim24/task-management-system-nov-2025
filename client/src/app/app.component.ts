import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [ HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  // protected readonly title = signal('client');
  title = 'Task Management System';
  tasks: any[] = [];

  ngOnInit(): void{
    this.http.get<any>(this.baseUrl + 'Tasks').subscribe({
      next: tasks => this.tasks = tasks.items,
      error: err => console.log(err),
      complete: () => console.log('Request completed')
    });
  }
}
