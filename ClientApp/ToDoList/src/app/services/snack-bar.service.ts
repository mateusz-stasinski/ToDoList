import { Component, Inject, Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarRef, MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackBarService {

  constructor(public snackBar: MatSnackBar) {}

  public open(message: string, className: string, duration: number = 3500) {
    this.snackBar.openFromComponent(SnackBarComponent, {data: message, duration: duration, panelClass: className})
  }
}

@Component({
  selector: 'snack-bar',
  template: `
    <div class="snackbar-content">
      <div>{{data}}</div>
      <button mat-raised-button (click)="snackBarRef.dismiss()" class="snack-bar-dismiss-button">&#10005;</button>
    </div>
  `
})

export class SnackBarComponent {
  constructor(
    public snackBarRef: MatSnackBarRef<SnackBarComponent>,
    @Inject(MAT_SNACK_BAR_DATA) public data: any
  ) {}
}