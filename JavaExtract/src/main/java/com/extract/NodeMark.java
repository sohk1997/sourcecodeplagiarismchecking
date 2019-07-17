package com.extract;

import com.github.javaparser.ast.Node;
import com.github.javaparser.ast.expr.SimpleName;
import javassist.Loader;

import java.util.ArrayList;
import java.util.List;

public class NodeMark {
    private Node node;
    private List<Node> references;
    private Node blockNode;

    public NodeMark(Node node, Node blockNode) {
        this.node = node;
        this.references = new ArrayList<>();
        this.references.add(node);
        this.blockNode = blockNode;
    }

    public void addReferences(Node node)
    {
        this.references.add(node);
    }

    public void renameAllReferences(String name)
    {
        for (Node node: this.references) {
            SimpleName simpleName = (SimpleName)node;
            simpleName.setIdentifier(name);
        }
    }

    public boolean isUsed()
    {
        return this.references.size() > 1;
    }

    public Node getNode()
    {
        return this.node;
    }

    public SimpleName getSimpleName()
    {
        return (SimpleName) this.node;
    }

    public Node getBlockNode()
    {
        return this.blockNode;
    }
}
