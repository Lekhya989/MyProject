import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        const expectedRoles = route.data['roles'] as Array<string>;
        if (!expectedRoles || expectedRoles.includes(role)) {
          return true;
        } else {
          this.router.navigate(['/login']); 
          return false;
        }
      } catch (err) {
        console.error("Invalid token", err);
        this.router.navigate(['/login']);
        return false;
      }
    }
    this.router.navigate(['/login']);
    return false;
  }
}