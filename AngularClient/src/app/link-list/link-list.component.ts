import { Component, OnInit, OnDestroy } from '@angular/core';
import { Link } from '../../models/link.model';
import { Apollo } from 'apollo-angular';

import {ALL_LINKS_QUERY, AllLinkQueryResponse} from '../graphql';
import { Subscription } from 'rxjs';
import { distinctUntilChanged } from 'rxjs/operators';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-link-list',
  templateUrl: './link-list.component.html',
  styleUrls: ['./link-list.component.css']
})
export class LinkListComponent implements OnInit, OnDestroy {
  allLinks: Link[] = [];
  loading = true;

  logged = false;

  subscriptions: Subscription[] = [];

  constructor(private apollo: Apollo, private authService: AuthService) {}

  ngOnInit() {
    this.authService.isAuthenticated.pipe(
      distinctUntilChanged()
    ).subscribe(isAuthenticated => {
      this.logged = isAuthenticated;
    });

    const querySubscription = this.apollo.watchQuery({
      query: ALL_LINKS_QUERY
    }).valueChanges.subscribe((response) => {
      this.allLinks = response.data.links;
      this.loading = false;
    });

    this.subscriptions = [...this.subscriptions, querySubscription];
  }

  updateStoreAfterVote (store, createVote, linkId) {
    const data = store.readQuery({
      query: ALL_LINKS_QUERY
    });
    const votedLink = data.links.find(link => link.id === linkId);
    votedLink.votes = createVote.link.votes;
    store.writeQuery({ query: ALL_LINKS_QUERY, data });
  }

  ngOnDestroy(): void {
    for (const sub of this.subscriptions) {
      if (sub && sub.unsubscribe) {
        sub.unsubscribe();
      }
    }
  }
}
