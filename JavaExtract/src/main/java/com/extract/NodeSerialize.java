package com.extract;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.github.javaparser.ast.Node;
import com.github.javaparser.ast.expr.BinaryExpr;
import com.github.javaparser.ast.expr.LiteralStringValueExpr;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

@JsonIgnoreProperties(value = {"baseNode"})
public class NodeSerialize implements Serializable {

    private String type;

    private int startLine;

    private int endLine;

    private String contextValue;

    private Node baseNode;

    @JsonProperty
    private List<NodeSerialize> childNodes;

    public NodeSerialize(Node baseNode) {
        this.baseNode = baseNode;
        this.contextValue = "";
        this.type = baseNode.getMetaModel().getTypeName();

        if (com.github.javaparser.ast.expr.LiteralStringValueExpr.class.isAssignableFrom(baseNode.getClass())) {
            LiteralStringValueExpr literalStringValueExpr = (LiteralStringValueExpr) baseNode;
            this.type += literalStringValueExpr.getValue();
        }

        if (com.github.javaparser.ast.expr.BinaryExpr.class.equals(baseNode.getClass())) {
            BinaryExpr binaryExpr = (BinaryExpr) baseNode;
            this.type += binaryExpr.getOperator().asString();
        }

        childNodes = new ArrayList<>();
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public int getStartLine() {
        return startLine;
    }

    public void setStartLine(int startLine) {
        this.startLine = startLine;
    }

    public int getEndLine() {
        return endLine;
    }

    public void setEndLine(int endLine) {
        this.endLine = endLine;
    }

    public void addChild(NodeSerialize node) {
        childNodes.add(node);
    }

    public void removeChild(int index) {
        childNodes.remove(index);
    }

    public Node getBaseNode() {
        return this.baseNode;
    }

    public NodeSerialize getChild(int index) {
        return childNodes.get(index);
    }

    public int countChild() {
        return childNodes.size();
    }

    public void setContextValue(String contextValue) {
        this.contextValue = contextValue;
    }
}
