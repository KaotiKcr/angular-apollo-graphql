export class Link {
  id: number;
  createdAt: string;
  updatedAt: string;
  description: string;
  url: string;
  postedBy?: User;
  votes?: [Vote];
}

export class User {
  id: number;
  createdAt: string;
  updatedAt: string;
  name: string;
  email: string;
  links?: [Link];
  votes?: [Vote];
}

export class Vote {
  id?: string;
  user?: User;
  link?: Link;
}
