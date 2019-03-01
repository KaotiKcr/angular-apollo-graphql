import { Component, OnInit, OnDestroy } from '@angular/core';
import { Link } from '../../models/link.model';
import { Apollo, QueryRef } from 'apollo-angular';

import {
  ALL_LINKS_QUERY,
  AllLinksQueryResponse,
  NEW_LINKS_SUBSCRIPTION,
  NEW_VOTES_SUBSCRIPTION,
  DELETE_LINKS_SUBSCRIPTION,
  AllLinksQueryVariable
} from '../graphql';
import { Subscription, Observable } from 'rxjs';
import { distinctUntilChanged, map } from 'rxjs/operators';
import { AuthService } from '../auth.service';
import { ActivatedRoute } from '@angular/router';
import { PAGE_SIZE } from '../constants';
import _ from 'lodash';

@Component({
  selector: 'app-link-list',
  templateUrl: './link-list.component.html',
  styleUrls: ['./link-list.component.css']
})
export class LinkListComponent implements OnInit, OnDestroy {
  allLinksQuery: QueryRef<any>;
  hasNextPage = false;
  cursor: String;
  allLinks: Link[] = [];
  loading = true;
  logged = false;
  linksPerPage = PAGE_SIZE;
  count = 0;

  subscriptions: Subscription[] = [];

  constructor(
    private apollo: Apollo,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.authService.isAuthenticated
      .pipe(distinctUntilChanged())
      .subscribe(isAuthenticated => {
        this.logged = isAuthenticated;
      });

    const getQuery = (): QueryRef<
      AllLinksQueryResponse,
      AllLinksQueryVariable
    > => {
      const query = this.apollo.watchQuery<
        AllLinksQueryResponse,
        AllLinksQueryVariable
      >({
        query: ALL_LINKS_QUERY,
        variables: {
          after: this.cursor,
          first: this.linksPerPage
        },
        fetchPolicy: 'network-only'
      });

      query.subscribeToMore({
        document: NEW_LINKS_SUBSCRIPTION,
        updateQuery: (previous, { subscriptionData }) => {
          if (!subscriptionData.data) {
            return previous;
          }

          const newLink = subscriptionData.data.linkCreated;

          return {
            ...previous,
            links: {
              items: [newLink, ...previous.links.items],
              __typename: previous.links.__typename,
              totalCount: previous.links.totalCount + 1,
              pageInfo: previous.links.pageInfo
            }
          };
        }
      });

      query.subscribeToMore({
        document: DELETE_LINKS_SUBSCRIPTION,
        updateQuery: (previous, { subscriptionData }) => {
          if (!subscriptionData.data) {
            return previous;
          }

          const deletedLink = subscriptionData.data.linkDeleted;
          const newAllLinks = previous.links.items.filter(
            item => item.id !== deletedLink.id
          );
          return {
            ...previous,
            links: {
              items: newAllLinks,
              __typename: previous.links.__typename,
              totalCount: previous.links.totalCount - 1,
              pageInfo: previous.links.pageInfo
            }
          };
        }
      });

      query.subscribeToMore({
        document: NEW_VOTES_SUBSCRIPTION,
        updateQuery: (previous, { subscriptionData }) => {
          if (!subscriptionData.data) {
            return previous;
          }

          const updatedLink = subscriptionData.data.voteCreated.link;
          const votedLinkIndex = previous.links.items.findIndex(link => {
            return link.id === updatedLink.id;
          });

          const newAllLinks = previous.links.items.slice();
          newAllLinks[votedLinkIndex] = updatedLink;

          return {
            ...previous,
            links: {
              items: newAllLinks,
              __typename: previous.links.__typename,
              totalCount: previous.links.totalCount,
              pageInfo: previous.links.pageInfo
            }
          };
        }
      });
      return query;
    };

    this.allLinksQuery = getQuery();
    const querySubscription = this.allLinksQuery.valueChanges.subscribe(
      response => {
        this.allLinks = response.data.links.items;
        this.count = response.data.links.totalCount;
        this.cursor = response.data.links.pageInfo.endCursor;
        this.hasNextPage = response.data.links.pageInfo.hasNextPage;
        this.loading = false;
      }
    );

    this.subscriptions = [...this.subscriptions, querySubscription];
  }

  get orderedLinks(): Observable<Link[]> {
    return this.route.url.pipe(map(segments => segments.toString())).pipe(
      map(path => {
        if (path.includes('top')) {
          return _.orderBy(this.allLinks, 'votes.length').reverse();
        } else {
          return _.orderBy(this.allLinks, 'id');
        }
      })
    );
  }

  get isFirstPage(): Observable<boolean> {
    return this.route.paramMap
      .pipe(
        map(params => {
          return params.get('after');
        })
      )
      .pipe(map(after => after === null));
  }

  get isNewPage(): Observable<boolean> {
    return this.route.url
      .pipe(map(segments => segments.toString()))
      .pipe(map(path => path.includes('new')));
  }

  // get pageAfter(): Observable<string> {
  //   return this.route.paramMap.pipe(
  //     map(params => {
  //       return params.get('after');
  //     })
  //   );
  // }

  // get morePages(): Observable<boolean> {
  //   return this.pageAfter.pipe(
  //     map(pageAfter => pageAfter < this.count / this.linksPerPage)
  //   );
  // }

  fetchMore() {
    if (this.hasNextPage) {
      this.allLinksQuery.fetchMore({
        query: ALL_LINKS_QUERY,
        variables: {
          after: this.cursor,
          first: this.linksPerPage
        },
        updateQuery: (previous, { fetchMoreResult }) => {
          if (!fetchMoreResult) {
            return previous;
          }

          const newLinks = fetchMoreResult.links.items.slice();
          newLinks.forEach(function(newLink) {
            if (!previous.links.items.find(link => link.id === newLink.id)) {
              previous.links.items.push(newLink);
            }
          });

          previous.links.totalCount = fetchMoreResult.links.totalCount;
          previous.links.pageInfo.endCursor =
            fetchMoreResult.links.pageInfo.endCursor;
          previous.links.pageInfo.hasNextPage =
            fetchMoreResult.links.pageInfo.hasNextPage;
          return previous;
        }
      });
    }
  }

  ngOnDestroy(): void {
    for (const sub of this.subscriptions) {
      if (sub && sub.unsubscribe) {
        sub.unsubscribe();
      }
    }
  }
}
