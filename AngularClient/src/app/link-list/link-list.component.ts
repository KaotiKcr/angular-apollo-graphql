import { Component, OnInit, OnDestroy } from '@angular/core';
import { Link } from '../../models/link.model';
import { Apollo } from 'apollo-angular';

import {
  ALL_LINKS_QUERY,
  AllLinksQueryResponse,
  NEW_LINKS_SUBSCRIPTION,
  NEW_VOTES_SUBSCRIPTION,
  DELETE_LINKS_SUBSCRIPTION
} from '../graphql';
import { Subscription, Observable, combineLatest } from 'rxjs';
import { distinctUntilChanged, map, switchMap, take } from 'rxjs/operators';
import { AuthService } from '../auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PAGE_SIZE } from '../constants';
import { ApolloQueryResult } from 'apollo-client';
import _ from 'lodash';

@Component({
  selector: 'app-link-list',
  templateUrl: './link-list.component.html',
  styleUrls: ['./link-list.component.css']
})
export class LinkListComponent implements OnInit, OnDestroy {
  allLinks: Link[] = [];
  loading = true;
  logged = false;
  linksPerPage = PAGE_SIZE;
  count = 0;

  subscriptions: Subscription[] = [];

  first$: Observable<number>;
  skip$: Observable<number>;

  constructor(
    private apollo: Apollo,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.authService.isAuthenticated
      .pipe(distinctUntilChanged())
      .subscribe(isAuthenticated => {
        this.logged = isAuthenticated;
      });

    const pageParams$: Observable<number> = this.route.paramMap.pipe(
      map(params => {
        return parseInt(params.get('page'), 10);
      })
    );

    const path$: Observable<string> = this.route.url.pipe(
      map(segments => segments.toString())
    );

    this.first$ = path$.pipe(
      map(path => {
        const isNewPage = path.includes('new');
        return isNewPage ? this.linksPerPage : 100;
      })
    );

    this.skip$ = combineLatest(path$, pageParams$).pipe(
      map(([path, page]) => {
        const isNewPage = path.includes('new');
        return isNewPage ? (page - 1) * this.linksPerPage : 0;
      })
    );

    const getQuery = (
      variables
    ): Observable<ApolloQueryResult<AllLinksQueryResponse>> => {
      const query = this.apollo.watchQuery<AllLinksQueryResponse>({
        query: ALL_LINKS_QUERY,
        variables
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
              __typename: previous.links.__typename
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
              __typename: previous.links.__typename
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
              __typename: previous.links.__typename
            }
          };
        }
      });
      return query.valueChanges;
    };

    // 6
    const allLinkQuery: Observable<
      ApolloQueryResult<AllLinksQueryResponse>
    > = combineLatest(this.first$, this.skip$, (first, skip) => ({
      first,
      skip
    })).pipe(switchMap((variables: any) => getQuery(variables)));

    const querySubscription = allLinkQuery.subscribe(response => {
      console.log(response);

      this.allLinks = response.data.links.items;
      this.count = response.data.links.totalCount;
      this.loading = false;
    });

    this.subscriptions = [...this.subscriptions, querySubscription];
  }

  updateStoreAfterVote(store, createVote, linkId) {
    let variables;

    combineLatest(this.first$, this.skip$, (first, skip) => ({ first, skip }))
      .pipe(take(1))
      .subscribe(values => (variables = values));

    const data = store.readQuery({
      query: ALL_LINKS_QUERY,
      variables
    });
    const votedLink = data.links.items.find(link => link.id === linkId);
    votedLink.votes = createVote.link.votes;

    store.writeQuery({ query: ALL_LINKS_QUERY, data });
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
    console.log('isFirstPage');
    return this.route.paramMap
      .pipe(
        map(params => {
          return parseInt(params.get('page'), 10);
        })
      )
      .pipe(map(page => page === 1));
  }

  get isNewPage(): Observable<boolean> {
    return this.route.url
      .pipe(map(segments => segments.toString()))
      .pipe(map(path => path.includes('new')));
  }

  get pageNumber(): Observable<number> {
    return this.route.paramMap.pipe(
      map(params => {
        return parseInt(params.get('page'), 10);
      })
    );
  }

  get morePages(): Observable<boolean> {
    return this.pageNumber.pipe(
      map(pageNumber => pageNumber < this.count / this.linksPerPage)
    );
  }

  nextPage() {
    const page = parseInt(this.route.snapshot.params.page, 10);
    if (page < this.count / PAGE_SIZE) {
      const nextPage = page + 1;
      this.router.navigate([`/new/${nextPage}`]);
    }
  }

  previousPage() {
    const page = parseInt(this.route.snapshot.params.page, 10);
    if (page > 1) {
      const previousPage = page - 1;
      this.router.navigate([`/new/${previousPage}`]);
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
