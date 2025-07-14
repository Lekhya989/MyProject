import { Component, OnInit } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../material/material.module';
import { RouterModule } from '@angular/router';

export interface NavigationItem{
  value: string;
  link:string;
  isActive : boolean;
  roles:string[];

}

@Component({
  selector: 'page-sidenav',
  standalone: true,
  imports:[CommonModule,MaterialModule, RouterModule], 
  templateUrl: './page-sidenav.component.html',
  styleUrl: './page-sidenav.component.scss'
})
export class PageSidenavComponent implements OnInit {
  panelName: string =' ';
  navItems: NavigationItem[]=[];
  filteredNavItems:NavigationItem[]=[];
  

  ngOnInit(): void {

    const token = localStorage.getItem('token');
    if (token) {
      const decoded: any = jwtDecode(token);
      const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      this.panelName = role === 'ADMIN' ? 'Admin Panel' : 'User Panel';
      this.filteredNavItems = this.navItems.filter(item => item.roles.includes(role));
    }

  }
 
  constructor() {
    this.navItems = [
      { value: "Time Slots", link: 'admin/time-slot', isActive: true, roles: ['ADMIN'] },
      { value: "My Bookings", link: 'dashboard/my-bookings', isActive: true, roles: ['USER'] },
      { value: "Slot Selection", link: 'dashboard/slot-selection', isActive: true, roles: ['USER'] },
      { value: "Manage Tax Pros", link: 'admin/manage-tax-pros', isActive: true, roles: ['ADMIN'] },
      { value: "User Bookings", link: 'admin/user-bookings', isActive: true, roles: ['ADMIN'] },
      {value: "Booking confirmation", link:'admin/booking-confirmation', isActive:true, roles:['ADMIN']},
      {value:"Admin Dashboard", link:'admin/admin-dashboard', isActive:true, roles:['ADMIN']},
      {value:"User Dashboard", link:'dashboard/user-dashboard', isActive:true, roles:['USER']},

    ];

  }
 
  logout() {
    localStorage.removeItem('token');
    location.href = '/login';
  }

}
 

