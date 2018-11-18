import { Component, OnInit } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { Router } from '@angular/router';

import { CREATE_LINK_MUTATION, CreateLinkMutationResponse } from '../graphql';
import { ALL_LINKS_QUERY, AllLinkQueryResponse } from '../graphql';

@Component({
  selector: 'app-create-link',
  templateUrl: './create-link.component.html',
  styleUrls: ['./create-link.component.css']
})
export class CreateLinkComponent implements OnInit {
  description = '';
  url = '';

  constructor(public apollo: Apollo, public router: Router) {}

  ngOnInit() {}

  createLink() {
    this.apollo
      .mutate<CreateLinkMutationResponse>({
        mutation: CREATE_LINK_MUTATION,
        variables: {
          link: {
            description: this.description,
            url: this.url,
            userId: 1
          }
        },
        update: (store, { data: { createLink } }) => {
          const data: any = store.readQuery({
            query: ALL_LINKS_QUERY
          });

          data.links.push(createLink);
          store.writeQuery({ query: ALL_LINKS_QUERY, data });
        }
      })
      .subscribe(response => {
        // We injected the Router service
        this.router.navigate(['/']);
      });
  }
}
