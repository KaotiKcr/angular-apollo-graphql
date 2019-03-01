import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { Link } from '../../models/link.model';

import {
  ALL_LINKS_QUERY,
  DELETE_LINK_MUTATION,
  DeleteLinkMutationResponse,
  CREATE_VOTE_MUTATION,
  AllLinksQueryResponse
} from '../graphql';
import { Apollo } from 'apollo-angular';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { distinctUntilChanged } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { GS_USER_ID } from '../constants';
import { DataProxy } from 'apollo-cache';
import { FetchResult } from 'apollo-link';

interface UpdateStoreAfterVoteCallback {
  (proxy: DataProxy, mutationResult: FetchResult, linkId: number);
}

@Component({
  selector: 'app-link-item',
  templateUrl: './link-item.component.html',
  styleUrls: ['./link-item.component.css']
})
export class LinkItemComponent implements OnInit, OnDestroy {
  @Input()
  link: Link;
  logged = false;

  @Input()
  isAuthenticated: false;
  subscriptions: Subscription[] = [];

  @Input()
  updateStoreAfterVote: UpdateStoreAfterVoteCallback;

  @Input()
  index = 0;

  constructor(
    public apollo: Apollo,
    public router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.authService.isAuthenticated
      .pipe(distinctUntilChanged())
      .subscribe(isAuthenticated => {
        this.logged = isAuthenticated;
      });
  }

  deleteLink() {
    this.apollo
      .mutate<DeleteLinkMutationResponse>({
        mutation: DELETE_LINK_MUTATION,
        variables: {
          id: this.link.id
        }
      })
      .subscribe(response => {
        // We injected the Router service
        this.router.navigate(['/']);
      });
  }

  voteForLink() {
    const userId = localStorage.getItem(GS_USER_ID);
    const voterIds = this.link.votes.map(vote => {
      return vote.user.id;
    });
    if (voterIds.includes(+userId)) {
      alert(`User (${userId}) already voted for this link.`);
      return;
    }
    const linkId = this.link.id;

    const mutationSubscription = this.apollo
      .mutate({
        mutation: CREATE_VOTE_MUTATION,
        variables: {
          userId,
          linkId
        }
      })
      .subscribe();

    this.subscriptions = [...this.subscriptions, mutationSubscription];
  }

  ngOnDestroy(): void {
    for (const sub of this.subscriptions) {
      if (sub && sub.unsubscribe) {
        sub.unsubscribe();
      }
    }
  }
}
