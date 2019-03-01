import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { Apollo } from 'apollo-angular';
import { Subscription } from 'rxjs';
import { Link } from 'src/models/link.model';
import { distinctUntilChanged } from 'rxjs/operators';

import { ALL_LINKS_QUERY, AllLinksQueryResponse } from '../graphql';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, OnDestroy {
  allLinks: Link[] = [];
  loading = true;
  searchText = '';
  logged = false;

  subscriptions: Subscription[] = [];

  constructor(private apollo: Apollo, private authService: AuthService) {}

  ngOnInit() {
    this.authService.isAuthenticated
      .pipe(distinctUntilChanged())
      .subscribe(isAuthenticated => {
        this.logged = isAuthenticated;
      });

    this.executeSearch();
  }

  executeSearch() {
    if (!this.searchText) {
      return;
    }

    const querySubscription = this.apollo
      .watchQuery<AllLinksQueryResponse>({
        query: ALL_LINKS_QUERY,
        variables: {
          searchText: this.searchText
        }
      })
      .valueChanges.subscribe(response => {
        this.allLinks = response.data.links.items;
        this.loading = false;
      });

    this.subscriptions = [...this.subscriptions, querySubscription];
  }

  ngOnDestroy(): void {
    for (let sub of this.subscriptions) {
      if (sub && sub.unsubscribe) {
        sub.unsubscribe();
      }
    }
  }
}
