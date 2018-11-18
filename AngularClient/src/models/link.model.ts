export class Link {
  id: number;
  createdAt: string;
  updatedAt: string;
  description: string;
  url: string;
  user: User;
}

export class User {
  id: number;
  createdAt: string;
  updatedAt: string;
  name: string;
  email: string;
  links: [Link];
}
