import { Component, OnInit, OnDestroy } from '@angular/core';
import { User } from 'src/models/link.model';
import { Apollo } from 'apollo-angular';
import { Subscription } from 'rxjs';
import { ALL_USERS_QUERY, AllUsersQueryResponse } from 'src/app/graphql';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit, OnDestroy {
  allUsers: User[] = [];
  loading = true;

  subscriptions: Subscription[] = [];

  constructor(public apollo: Apollo) {}

  ngOnInit() {
    const querySubscription = this.apollo
      .watchQuery<AllUsersQueryResponse>({
        query: ALL_USERS_QUERY
      })
      .valueChanges.subscribe(response => {
        this.allUsers = response.data.users.items;
        this.loading = false;
      });

    this.subscriptions = [...this.subscriptions, querySubscription];
  }

  ngOnDestroy() {
    for (const sub of this.subscriptions) {
      if (sub && sub.unsubscribe) {
        sub.unsubscribe();
      }
    }
  }
}
