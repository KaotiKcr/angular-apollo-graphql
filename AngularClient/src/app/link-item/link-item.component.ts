import { Component, Input, OnInit } from '@angular/core';
import { Link } from '../../models/link.model';

import { DELETE_LINK_MUTATION, DeleteLinkMutationResponse } from '../graphql';
import { ALL_LINKS_QUERY } from '../graphql';
import { Apollo } from 'apollo-angular';
import { Router } from '@angular/router';


@Component({
  selector: 'app-link-item',
  templateUrl: './link-item.component.html',
  styleUrls: ['./link-item.component.css']
})
export class LinkItemComponent implements OnInit {
  @Input()
  link: Link;

  constructor(public apollo: Apollo, public router: Router) {}

  ngOnInit() {}

  deleteLink() {
    this.apollo
      .mutate<DeleteLinkMutationResponse>({
        mutation: DELETE_LINK_MUTATION,
        variables: {
          id: this.link.id
        },
        update: (store, { data: { deleteLink } }) => {
          const data: any = store.readQuery({
            query: ALL_LINKS_QUERY
          });

          data.links = data.links.filter(item => item.id !== deleteLink.id);
          store.writeQuery({ query: ALL_LINKS_QUERY, data });
        }
      })
      .subscribe(response => {
        // We injected the Router service
        this.router.navigate(['/']);
      });
  }

  voteForLink = async () => {
    // ... you'll implement this in chapter 6
  }
}
