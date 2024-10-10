import { Component, inject, OnInit, signal } from '@angular/core';
import { User, UserService } from '../../services/user.service';

@Component({
  selector: 'app-list-users',
  standalone: true,
  imports: [],
  templateUrl: './list-users.component.html',
  styleUrl: './list-users.component.css'
})
export class ListUsersComponent implements OnInit {
  private userUservice = inject(UserService);
  userList = signal<User[]>([]);

  ngOnInit(): void {
    this.userUservice.getUsers().subscribe({
      next: (result) => {
        this.userList.set(result);
      }
    })
  }

}
