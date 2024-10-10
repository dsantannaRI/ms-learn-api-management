import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-user',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-user.component.html',
  styleUrl: './create-user.component.css'
})
export class CreateUserComponent {
  private route = inject(Router);
  private userService = inject(UserService)

  user = {
    fullname: '',
    email: ''
  };

  onSubmit() {

    this.userService.createUser(this.user).subscribe({
      next: (result) => {
        window.alert('user added')
        this.route.navigateByUrl('/list-users')
      },
      error: (e) => {
        e.error.forEach((element: string) => {
          window.alert(element);
        });
        console.log('error', e);
      }
    })
  }
}
