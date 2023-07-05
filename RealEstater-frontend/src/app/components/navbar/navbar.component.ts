import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  public fullName!: string;

  constructor(private auth: AuthService, 
              private userStore: UserStoreService, 
              private http: HttpClient,
              private router: Router) { }

  ngOnInit(): void {
    this.fullName = this.auth.getFullNameFromToken();
  }

  logout(){
    this.auth.signOut();
  }

  profile(){
    this.router.navigate(['profile']);
  }

  home(){
    this.router.navigate(["/dashboard"]);
  }

  userLandholdings(){
    this.router.navigate(["/user-landholdings"]);
  }

  search(){
    let query = document.getElementById('searchInput') as HTMLInputElement;
    this.redirectTo('/search/' + `${query?.value ? query?.value : 'all'}`);
  }
  
  messages(){
    this.redirectTo('/myMessages');
  }

  redirectTo(uri:string){
    this.router.navigateByUrl('/', {skipLocationChange: true}).then(()=>
    this.router.navigate([uri]));
 }
}
