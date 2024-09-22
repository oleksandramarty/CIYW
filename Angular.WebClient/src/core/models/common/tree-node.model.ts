export interface ITreeNodeResponse<TNode> {
  node: TNode;
  parent?: TNode;
  children: TNode[];
}

export class TreeNodeResponse<TNode> implements ITreeNodeResponse<TNode> {
  node: TNode;
  parent?: TNode;
  children: TNode[];

  constructor(node: TNode, parent?: TNode, children: TNode[] = []) {
    this.node = node;
    this.parent = parent;
    this.children = children;
  }
}
