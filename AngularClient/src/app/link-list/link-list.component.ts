import { Component, OnInit } from '@angular/core';
import { Link } from '../../models/link.model';
import { Apollo } from 'apollo-angular';

import {ALL_LINKS_QUERY, AllLinkQueryResponse} from '../graphql';

@Component({
  selector: 'app-link-list',
  templateUrl: './link-list.component.html',
  styleUrls: ['./link-list.component.css']
})
export class LinkListComponent implements OnInit {
  allLinks: Link[] = [];
  loading = true;

  constructor(private apollo: Apollo) {}

  ngOnInit() {
    this.apollo.watchQuery<AllLinkQueryResponse>({
      query: ALL_LINKS_QUERY
    }).valueChanges.subscribe((response) => {
      this.allLinks = response.data.Links;
      this.loading = false;
     });
  }
}
